using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.Commands;
using Godzilla.Queries;
using Godzilla.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Internal
{
    internal class EntityContextServiceBuilder<TContext>
        where TContext : EntityContext
    {
        private readonly IServiceCollection services;

        public EntityContextServiceBuilder(IServiceCollection services)
        {
            this.services = services;
        }

        public IServiceCollection Build()
        {
            AddLibraries();
            AddCoreServices();
            AddCommandHandlers();
            AddInternalServices();
            return services;
        }

        private void AddCoreServices()
        {
            services
                .AddScoped<ICollectionInitializer, CollectionInitializer>()
                .AddScoped<ICollectionResolver<TContext>, CollectionResolver<TContext>>()
                .AddScoped<ICollectionService<TContext>, CollectionService<TContext>>()
                .AddScoped<IEntityPropertyResolver<TContext>, EntityPropertyResolver<TContext>>()
                .AddTransient<ITransactionService<TContext>, TransactionService<TContext>>()
                .AddScoped<IEntityContextServices<TContext>, EntityContextServices<TContext>>()
                .AddScoped<IEntityCommandsHelper<TContext>, CommandHandlerHelper<TContext>>()
                .AddScoped<TContext>();
        }

        private void AddCommandHandlers()
        {
            services
                .AddTransient<IRequestHandler<CreateEntitiesCommand<TContext>, IEnumerable<object>>, CreateEntitiesCommandHandler<TContext>>()
                .AddTransient<IRequestHandler<UpdateEntitiesCommand<TContext>, IEnumerable<object>>, UpdateEntitiesCommandHandler<TContext>>()
                .AddTransient<IRequestHandler<DeleteEntitiesCommand<TContext>>, DeleteEntitiesCommandHandler<TContext>>();
        }

        private void AddInternalServices()
        {
            services
                .AddScoped<EntityQueries<TContext>>()
                .AddScoped<EntityCommands<TContext>>();
        }

        private void AddLibraries()
        {
            services
                .AddMediatR();
        }
    }
}
