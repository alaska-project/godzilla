using Godzilla.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions
{
    public interface IEntityContextServices<TContext> : IEntityContextServices
        where TContext : EntityContext
    {
    }

    public interface IEntityContextServices
    {
        ICollectionService Collections { get; }
        IEntityCommands Commands { get; }
        IEntityQueries Queries { get; }
        IPathBuilder PathBuilder { get; }
        IEntityPropertyResolver PropertyResolver { get; }
        IEntityConfigurator Configurator { get; }
        IEntityContextInitializer Initializer { get; }
        IEntityNotificationService NotificationService { get; }
    }
}
