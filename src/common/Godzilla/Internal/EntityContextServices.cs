using Godzilla.Abstractions;
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
            EntityCommands<TContext> commands)
        {
            Queryes = queries ?? throw new ArgumentNullException(nameof(queries));
            Commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        public IEntityCommands Commands { get; }

        public IEntityQueries Queryes { get; }
    }
}
