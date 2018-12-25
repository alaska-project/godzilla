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
        IEntityCommands Commands { get; }
        IEntityQueries Queryes { get; }
    }
}
