using Godzilla.Abstractions;
using Godzilla.AspNetCore.Security;
using Godzilla.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public static class AspNetCoreDependencyInjectionExtensions
    {
        public static IEntityContextServiceCollection<TContext> AddEntityContextAuthorization<TContext>(this IEntityContextServiceCollection<TContext> services)
            where TContext : EntityContext
        {
            var options = new SecurityOptions<TContext>();

            services.Services
                .AddSingleton<ISecurityOptions<TContext>>(options)
                .AddScoped<ISecurityContextProvider<TContext>, AspNetCoreSecurityProvider<TContext>>();

            return services;
        }
    }
}
