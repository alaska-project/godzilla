using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.Commands;
using Godzilla.Notifications;
using Godzilla.Queries;
using Godzilla.Security;
using Godzilla.Services;
using Godzilla.Settings;
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
        private EntityContextResolver _EntityContextResolver = EntityContextResolver.Instance;

        private readonly IGodzillaServiceBuilder _builder;
        private readonly SecurityOptions<TContext> _securityOptions;

        public EntityContextServiceBuilder(IGodzillaServiceBuilder builder, SecurityOptions<TContext> securityOptions)
        {
            _builder = builder;
            _securityOptions = securityOptions ?? throw new ArgumentNullException(nameof(securityOptions));
        }
        
        public IEntityContextServiceCollection<TContext> Build()
        {
            AddLibraries();
            AddCoreServices();
            AddContextRegistry();
            AddCommandHandlers();
            AddInternalServices();
            AddSecurityServices();

            return new EntityContextServiceCollection<TContext>(_builder.Services);
        }

        private void AddCoreServices()
        {
            _builder.Services
                .AddSingleton<IGodzillaOptions<TContext>, GodzillaOptions<TContext>>()
                .AddScoped<ICollectionInitializer, CollectionInitializer>()
                .AddScoped<IPathBuilder<TContext>, PathBuilder<TContext>>()
                .AddScoped<ICollectionResolver<TContext>, CollectionResolver<TContext>>()
                .AddScoped<ICollectionService<TContext>, CollectionService<TContext>>()
                .AddScoped<IEntityPropertyResolver<TContext>, EntityPropertyResolver<TContext>>()
                .AddTransient<ITransactionService<TContext>, TransactionService<TContext>>()
                .AddScoped<IEntityContextServices<TContext>, EntityContextServices<TContext>>()
                .AddScoped<IEntityCommandsHelper<TContext>, CommandHandlerHelper<TContext>>()
                .AddSingleton<IEntityNotificationService<TContext>, EntityNotificationService<TContext>>()
                .AddScoped<IEntityCollectionsService<TContext>, EntityCollectionsService<TContext>>()
                .AddScoped<TContext>();
        }

        private void AddSecurityServices()
        {
            _builder.Services
                .AddScoped<ISecurityRulesFinder<TContext>, SecurityRulesFinder<TContext>>()
                .AddScoped<ISecurityRuleEvaluator<TContext>, SecurityRuleEvaluator<TContext>>()
                .AddScoped<ISecurityContext<TContext>, SecurityContext<TContext>>()
                .AddScoped<ISecurityRuleMatcher<TContext>, SecurityRuleMatcher<TContext>>()
                .AddSingleton<ISecurityOptions<TContext>>(_securityOptions);
        }

        private void AddCommandHandlers()
        {
            _builder.Services
                .AddTransient<IRequestHandler<CreateEntitiesCommand<TContext>, IEnumerable<object>>, CreateEntitiesCommandHandler<TContext>>()
                .AddTransient<IRequestHandler<UpdateEntitiesCommand<TContext>, IEnumerable<object>>, UpdateEntitiesCommandHandler<TContext>>()
                //.AddTransient(typeof(IRequestHandler<PartialUpdateEntitiesCommand<,,>, PartialUpdateEntitiesCommandHandler<,,>>))
                .AddTransient<IRequestHandler<MoveEntityCommand<TContext>, Unit>, MoveEntityCommandHandler<TContext>>()
                .AddTransient<IRequestHandler<RenameEntityCommand<TContext>, Unit>, RenameEntityCommandHandler<TContext>>()
                .AddTransient<IRequestHandler<DeleteEntitiesCommand<TContext>, Unit>, DeleteEntitiesCommandHandler<TContext>>()
                .AddTransient<IRequestHandler<SetPermissionsCommand<TContext>, Unit>, SetPermissionsCommandHandler<TContext>>();
        }

        private void AddInternalServices()
        {
            _builder.Services
                .AddSingleton<EntityContextInitializer<TContext>>()
                .AddScoped<EntityConfigurator<TContext>>()
                .AddScoped<EntityQueries<TContext>>()
                .AddScoped<EntityCommands<TContext>>();
        }

        private void AddContextRegistry()
        {
            _EntityContextResolver.RegisterContext<TContext>();

            if (!_builder.Services.Contains(new ServiceDescriptor(typeof(IEntityContextResolver), _EntityContextResolver)))
            {
                _builder.Services.AddSingleton(typeof(IEntityContextResolver), _EntityContextResolver);
            }
        }

        private void AddLibraries()
        {
            _builder.Services
                .AddMediatR();
        }
    }
}
