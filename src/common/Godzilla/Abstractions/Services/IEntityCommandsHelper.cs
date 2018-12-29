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
        IEnumerable<TreeEdge> VerifyEntitiesExist(IEnumerable<object> entities, TreeEdgesCollection edgesCollection);
        IEnumerable<TreeEdge> VerifyEntitiesExist(IEnumerable<Guid> entitiesId, TreeEdgesCollection edgesCollection);
        IEnumerable<Guid> GetEntitiesId(IEnumerable<object> entities);
        string BuildNamePath(string name, TreeEdge parent);
        string BuildIdPath(Guid id, TreeEdge parent);
    }
}
