using Godzilla.Abstractions.Collections;
using Godzilla.Abstractions.Infrastructure;
using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Collections.Internal
{
    internal class TreeEdgesCollection : GodzillaCollection<TreeEdge>,
        ITreeEdgesCollection
    {
        /// <summary>
        /// For testing purpose only
        /// </summary>
        internal TreeEdgesCollection()
            : base(null)
        { }

        public TreeEdgesCollection(IDatabaseCollection<TreeEdge> collection)
            : base(collection)
        { }

        public virtual IEnumerable<TreeEdge> GetNodes(IEnumerable<Guid> nodesId)
        {
            return _collection
                .AsQueryable()
                .Where(x => nodesId.Contains(x.NodeId))
                .ToList();
        }

        public virtual bool ExistsAny(IEnumerable<Guid> nodesId)
        {
            return _collection
                .AsQueryable()
                .Any(x => nodesId.Contains(x.NodeId));
        }

        public virtual bool NodeExists(Guid nodeId)
        {
            return _collection
                .AsQueryable()
                .Any(x => x.NodeId == nodeId);
        }
    }
}
