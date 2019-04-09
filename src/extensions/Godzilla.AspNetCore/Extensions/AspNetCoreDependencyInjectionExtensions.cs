using Godzilla.Abstractions;
using Godzilla.AspNetCore.Security;
using Godzilla.AspNetCore.Ui.Abstractions;
using Godzilla.AspNetCore.Ui.Query;
using Godzilla.AspNetCore.Ui.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public static class AspNetCoreDependencyInjectionExtensions
    {
        public static IEntityContextServiceCollection<TContext> AddMvcAuthentication<TContext>(this IEntityContextServiceCollection<TContext> services)
            where TContext : EntityContext
        {
            services.Services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<ISecurityContextProvider<TContext>, AspNetCoreSecurityProvider<TContext>>();

            return services;
        }

        public static IEntityContextServiceCollection<TContext> AddUi<TContext>(this IEntityContextServiceCollection<TContext> services)
            where TContext : EntityContext
        {
            services.Services
                .AddScoped<UiNodeConverter>()
                .AddTransient<IUiQuery, UiQuery>();

            services.Services
                .AddMvcCore()
                .AddApplicationPart(typeof(AspNetCoreDependencyInjectionExtensions).Assembly);

            return services;
        }
    }
}
