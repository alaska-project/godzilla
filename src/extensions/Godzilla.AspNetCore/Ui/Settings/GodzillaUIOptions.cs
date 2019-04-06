using Godzilla.AspNetCore.Ui.Middlewares;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Godzilla.AspNetCore.Ui.Settings
{
    public class GodzillaUIOptions : IAngularUIOptions
    {
        public string RoutePrefix { get; set; } = "godzilla";

        public bool Rewrite404ToIndexHtml { get; set; } = true;

        public List<string> ExcludedRoutes { get; set; } = new List<string> { "api" };

        public string ManifestResourceBasePath { get; set; } = "Godzilla.AspNetCore.Ui.Assets";

        public Assembly ManifestResourceAssembly { get; set; } = typeof(GodzillaUIOptions).GetTypeInfo().Assembly;

        public List<string> Endpoints { get; set; } = new List<string>();

        public string DocumentTitle { get; set; } = "Godzilla";

        public string HeadContent { get; set; } = "";

        public JObject ConfigObject { get; } = JObject.FromObject(new
        {
            urls = new object[] { },
            validatorUrl = JValue.CreateNull()
        });

        public JObject OAuthConfigObject { get; } = JObject.FromObject(new
        {
        });
    }
}
