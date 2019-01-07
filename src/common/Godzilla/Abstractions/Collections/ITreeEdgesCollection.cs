using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Collections
{
    internal interface ITreeEdgesCollection
    {
        TreeEdge GetNode(Guid nodeId);
        IEnumerable<TreeEdge> GetNodes(IEnumerable<Guid> nodesId);
        bool ExistsAny(IEnumerable<Guid> nodesId);
        bool NodeExists(Guid nodeId);
        void DeleteNode(Guid nodeId);
        void DeleteNodes(IEnumerable<Guid> nodesId);
        IEnumerable<TreeEdge> GetDescendants(TreeEdge node);
    }
}
