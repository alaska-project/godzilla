using Godzilla.Abstractions.Collections;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    internal interface IEntityCommandsHelper<TContext>
        where TContext : EntityContext
    {
        Type GetEntityType(IEnumerable<object> entities);
        IEnumerable<EntityNode> VerifyEntitiesExist<TEntity>(IEnumerable<TEntity> entities, EntityNodesCollection edgesCollection);
        IEnumerable<EntityNode> VerifyEntitiesExist(IEnumerable<object> entities, EntityNodesCollection edgesCollection);
        IEnumerable<EntityNode> VerifyEntitiesExist(IEnumerable<Guid> entitiesId, EntityNodesCollection edgesCollection);
        IEnumerable<Guid> GetEntitiesId<TEntity>(IEnumerable<TEntity> entities);
        IEnumerable<Guid> GetEntitiesId(IEnumerable<object> entities);
        string BuildNamePath(string name, EntityNode parent);
        string BuildIdPath(Guid id, EntityNode parent);
    }
}
