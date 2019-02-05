using Godzilla.Abstractions;
using Godzilla.Internal;
using Godzilla.Security;
using Godzilla.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public static class GodzillaDependencyInjectionExtensions
    {
        public static IGodzillaServiceBuilder AddGodzilla(this IServiceCollection services)
        {
            services.AddScoped<ISecurityImpersonationService, SecurityImpersonationService>();

            return new GodzillaServiceBuilder(services);
        }

        public static IEntityContextServiceCollection<TContext> AddEntityContext<TContext>(this IGodzillaServiceBuilder builder, Action<EntityContextOptionsBuilder<TContext>> optionsBuilder = null)
            where TContext : EntityContext
        {
            var optionsBuilderObj = new EntityContextOptionsBuilder<TContext>(builder);
            optionsBuilder?.Invoke(optionsBuilderObj);

            return new EntityContextServiceBuilder<TContext>(builder)
                .Build();
        }
    }
}
