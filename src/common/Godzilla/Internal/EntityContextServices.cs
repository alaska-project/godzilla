using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.Queries;
using Godzilla.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Internal
{
    internal class EntityContextServices<TContext> : 
        IEntityContextServices,
        IEntityContextServices<TContext>
        where TContext : EntityContext
    {
        public EntityContextServices(
            ILogger<TContext> logger,
            ICollectionService<TContext> collectionsService, 
            EntityQueries<TContext> queries,
            EntityCommands<TContext> commands,
            EntityConfigurator<TContext> configurator,
            IEntityPropertyResolver<TContext> propertyResolver,
            IPathBuilder<TContext> pathBuilder,
            EntityContextInitializer<TContext> initializer,
            IEntityNotificationService<TContext> notificationService)
        {
            Logger = logger;
            Collections = collectionsService ?? throw new ArgumentException(nameof(collectionsService));
            Queries = queries ?? throw new ArgumentNullException(nameof(queries));
            Commands = commands ?? throw new ArgumentNullException(nameof(commands));
            Configurator = configurator ?? throw new ArgumentNullException(nameof(configurator));
            PropertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
            PathBuilder = pathBuilder ?? throw new ArgumentNullException(nameof(pathBuilder));
            Initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            NotificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        public ILogger Logger { get; }

        public ICollectionService Collections { get; }

        public IEntityCommands Commands { get; }

        public IEntityQueries Queries { get; }

        public IEntityConfigurator Configurator { get; }

        public IEntityContextInitializer Initializer { get; }

        public IEntityNotificationService NotificationService { get; }

        public IEntityPropertyResolver PropertyResolver { get; }

        public IPathBuilder PathBuilder { get; }
    }
}
