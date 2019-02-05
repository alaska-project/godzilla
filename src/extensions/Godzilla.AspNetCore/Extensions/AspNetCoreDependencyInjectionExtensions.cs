﻿using Godzilla.Abstractions;
using Godzilla.AspNetCore.Security;
using Godzilla.Internal;
using Godzilla.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public static class AspNetCoreDependencyInjectionExtensions
    {
        public static IEntityContextServiceCollection<TContext> AddAuthorization<TContext>(this IEntityContextServiceCollection<TContext> services)
            where TContext : EntityContext
        {
            EntityContextServiceBuilder<TContext>.SecurityOptions.UseAuthorization = true;

            services.Services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<ISecurityContextProvider<TContext>, AspNetCoreSecurityProvider<TContext>>();

            return services;
        }
    }
}
