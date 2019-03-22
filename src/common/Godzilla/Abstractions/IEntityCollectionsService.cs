using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions
{
    internal interface IEntityCollectionsService<TContext>
        where TContext : EntityContext
    {
        EntityNodesCollection GetEntityNodesCollection();
        IGodzillaCollection<TEntity> GetCollection<TEntity>();
    }
}
