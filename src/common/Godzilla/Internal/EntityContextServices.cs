﻿using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.Queries;
using Godzilla.Services;
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
            EntityQueries<TContext> queries,
            EntityCommands<TContext> commands,
            EntityConfigurator<TContext> configurator,
            EntityContextInitializer<TContext> initializer)
        {
            Queryes = queries ?? throw new ArgumentNullException(nameof(queries));
            Commands = commands ?? throw new ArgumentNullException(nameof(commands));
            Configurator = configurator ?? throw new ArgumentNullException(nameof(configurator));
            Initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
        }

        public IEntityCommands Commands { get; }

        public IEntityQueries Queryes { get; }

        public IEntityConfigurator Configurator { get; }

        public IEntityContextInitializer Initializer { get; }
    }
}
