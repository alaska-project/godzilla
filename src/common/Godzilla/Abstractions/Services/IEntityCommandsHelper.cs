using Godzilla.Abstractions.Collections;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Services
{
    internal interface IEntityCommandsHelper<TContext>
        where TContext : EntityContext
    {
        Type GetEntityType(IEnumerable<object> entities);
        Task<IEnumerable<EntityNode>> VerifyEntities<TEntity>(IEnumerable<TEntity> entities, EntityNodesCollection edgesCollection, Guid permission);
        Task<IEnumerable<EntityNode>> VerifyEntities(IEnumerable<object> entities, EntityNodesCollection edgesCollection, Guid permission);
        Task<IEnumerable<EntityNode>> VerifyEntities(IEnumerable<Guid> entitiesId, EntityNodesCollection edgesCollection, Guid permission);
        Task<EntityNode> VerifyEntity(Guid entitiyId, EntityNodesCollection edgesCollection, Guid permission);
        Task VerifyRootNodePermission(Guid permission);
        IEnumerable<Guid> GetEntitiesId<TEntity>(IEnumerable<TEntity> entities);
        IEnumerable<Guid> GetEntitiesId(IEnumerable<object> entities);
        string BuildNamePath(string name, EntityNode parent);
        string BuildIdPath(Guid id, EntityNode parent);
    }
}
