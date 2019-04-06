using Godzilla.AspNetCore.Ui.Middlewares;
using Godzilla.AspNetCore.Ui.Settings;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public static class AspNetCoreApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGodzillaUI(this IApplicationBuilder app, Action<GodzillaUIOptions> setupAction = null)
        {
            var options = new GodzillaUIOptions();
            setupAction?.Invoke(options);

            GodzillaUIOptionsRepository.Options = options;

            app.UseMiddleware<GodzillaUIIndexMiddleware>(options);

            return app;
        }
    }
}
