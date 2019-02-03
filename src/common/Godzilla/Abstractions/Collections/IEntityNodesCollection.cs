using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Collections
{
    internal interface IEntityNodesCollection
    {
        EntityNode GetNode(Guid nodeId);
        IEnumerable<EntityNode> GetNodes(IEnumerable<Guid> nodesId);
        bool ExistsAny(IEnumerable<Guid> nodesId);
        bool NodeExists(Guid nodeId);
        Task DeleteNode(Guid nodeId);
        Task DeleteNodes(IEnumerable<Guid> nodesId);
        IEnumerable<EntityNode> GetDescendants(EntityNode node);
    }
}
