using Godzilla.Abstractions;
using Godzilla.Internal;
using Godzilla.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public static class GodzillaStartupExtensions
    {
        public static IEntityContextServiceCollection<TContext> AddEntityContext<TContext>(this IServiceCollection services, Action<EntityContextOptionsBuilder> optionsBuilder = null)
            where TContext : EntityContext
        {
            var optionsBuilderObj = new EntityContextOptionsBuilder(services);
            optionsBuilder?.Invoke(optionsBuilderObj);

            return new EntityContextServiceBuilder<TContext>(services)
                .Build();
        }
    }
}
