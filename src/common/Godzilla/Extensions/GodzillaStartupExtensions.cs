using Godzilla.Abstractions.Services;
using Godzilla.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public static class GodzillaStartupExtensions
    {
        public static IServiceCollection AddEntityContext<TContext>(this IServiceCollection services, Action<EntityContextOptionsBuilder> optionsBuilder = null)
            where TContext : EntityContext
        {
            var optionsBuilderObj = new EntityContextOptionsBuilder(services);
            optionsBuilder?.Invoke(optionsBuilderObj);

            return services
                .AddScoped<ICollectionInitializer, CollectionInitializer>()
                .AddScoped<ICollectionResolver<TContext>, CollectionResolver<TContext>>()
                .AddScoped<ICollectionService<TContext>, CollectionService<TContext>>()
                .AddScoped<IEntityPropertyResolver<TContext>, EntityPropertyResolver<TContext>>()
                .AddScoped<ITransactionService<TContext>, TransactionService<TContext>>();
        }
    }
}
