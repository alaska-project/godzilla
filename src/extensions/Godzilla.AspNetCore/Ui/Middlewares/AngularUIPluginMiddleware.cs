using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Godzilla.AspNetCore.Ui.Middlewares
{
    public class AngularUIPluginMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAngularUIOptions _options;

        public AngularUIPluginMiddleware(RequestDelegate next, IAngularUIOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            // If the RoutePrefix is requested (with or without trailing slash), redirect to index URL
            if (IsIndexHtml(path))
            {
                await RespondWithIndexHtml(httpContext.Response);
                return;
            }

            if (httpMethod == "GET" && 
                MatchesEmbeddedContentRootPath(path) &&
                !MatchesEmbeddedContentExcludedPaths(path))
            {
                var relativeContentPath = path.Substring(_options.RoutePrefix.Length + 1);
                await RespondWithEmbeddedContent(httpContext.Response, relativeContentPath);
                return;
            }

            await _next(httpContext);
            return;
        }

        private bool IsIndexHtml(string path)
        {
            return Regex.IsMatch(path, $"^/{_options.RoutePrefix}/?$");
        }

        private bool MatchesEmbeddedContentRootPath(string path)
        {
            return Regex.IsMatch(path, $"^/{_options.RoutePrefix}/?");
        }

        private bool MatchesEmbeddedContentExcludedPaths(string path)
        {
            return _options.ExcludedRoutes.Any(x =>
                Regex.IsMatch(path, $"^/{_options.RoutePrefix}/{x}/?"));
        }

        private void RespondWithRedirect(HttpResponse response, string redirectPath)
        {
            response.StatusCode = 301;
            response.Headers["Location"] = redirectPath;
        }

        private async Task RespondWithEmbeddedContent(HttpResponse response, string relativeContentPath)
        {
            response.StatusCode = 200;
            response.ContentType = GetContentType(relativeContentPath);

            var content = GetEmbeddedResource(relativeContentPath);
            if (content != null)
            {
                await response.WriteAsync(content, Encoding.UTF8);
                return;
            }
                

            if (_options.Rewrite404ToIndexHtml)
            {
                await response.WriteAsync(GetIndexHtmlContent(), Encoding.UTF8);
                return;
            }

            response.StatusCode = 404;
            await response.WriteAsync(string.Empty);
        }

        private string GetContentType(string relativeContentPath)
        {
            var extension = relativeContentPath.Split('.').LastOrDefault().ToLower();
            switch (extension)
            {
                case "css":
                    return "text/css";
                case "js":
                    return "text/javascript";
                case "gif":
                    return "image/gif";
                case "png":
                    return "image/png";
                case "eot":
                    return "application/vnd.ms-fontobject";
                case "woff":
                    return "application/font-woff";
                case "woff2":
                    return "application/font-woff2";
                case "otf":
                    return "application/font-sfnt"; // formerly "font/opentype"
                case "ttf":
                    return "application/font-sfnt"; // formerly "font/truetype"
                case "svg":
                    return "image/svg+xml";
                default:
                    return "text/html";
            }
        }

        private async Task RespondWithIndexHtml(HttpResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html";

            // Inject parameters before writing to response
            var content = GetIndexHtmlContent();
            var htmlBuilder = new StringBuilder(content);
            foreach (var entry in GetIndexParameters())
            {
                htmlBuilder.Replace(entry.Key, entry.Value);
            }

            var indexPageContent = htmlBuilder.ToString();
            
            await response.WriteAsync(indexPageContent, Encoding.UTF8);
        }

        private string GetIndexHtmlContent()
        {
            return GetEmbeddedResource("index.html");
        }

        private string GetEmbeddedResource(string relativeResourcePath)
        {
            var manifestResourcePath = $"{_options.ManifestResourceBasePath}.{relativeResourcePath.TrimStart('/').Replace("/", ".")}";

            using (var stream = _options.ManifestResourceAssembly.GetManifestResourceStream(manifestResourcePath))
            {
                if (stream == null)
                    return null;

                using (var reader = new StreamReader(stream))
                {
                    var sb = new StringBuilder(reader.ReadToEnd());
                    return sb.ToString();
                }
            }
        }

        private IDictionary<string, string> GetIndexParameters()
        {
            return new Dictionary<string, string>()
            {
                { "%(DocumentTitle)", _options.DocumentTitle },
                { "%(HeadContent)", _options.HeadContent },
                { "%(ConfigObject)", SerializeToJson(_options.ConfigObject) },
                { "%(OAuthConfigObject)", SerializeToJson(_options.OAuthConfigObject) }
            };
        }

        private string ReplaceBaseHref(string indexPageContent)
        {
            return indexPageContent.Replace("<base href=\"/\">", $"<base href=\"/{_options.RoutePrefix}\">");
        }

        private string SerializeToJson(JObject jObject)
        {
            return JsonConvert.SerializeObject(jObject, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                Formatting = Formatting.None
            });
        }
    }

    public interface IAngularUIOptions
    {
        string RoutePrefix { get; }
        bool Rewrite404ToIndexHtml { get; }
        List<string> ExcludedRoutes { get; }
        string ManifestResourceBasePath { get; }
        Assembly ManifestResourceAssembly { get; }
        List<string> Endpoints { get; }
        string DocumentTitle { get; }
        string HeadContent { get; }
        JObject ConfigObject { get; }
        JObject OAuthConfigObject { get; }
    }
}
