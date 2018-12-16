using Godzilla.Abstractions.Infrastructure;
using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Collections.Internal
{
    internal class TreeEdgesCollection : GodzillaCollection<TreeEdge>
    {
        public TreeEdgesCollection(IDatabaseCollection<TreeEdge> collection)
            : base(collection)
        { }

        public bool NodeExists(Guid nodeId)
        {
            return _collection
                .AsQueryable()
                .Any(x => x.NodeId == nodeId);
        }
    }
}
