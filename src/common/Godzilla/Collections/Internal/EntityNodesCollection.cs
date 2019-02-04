using Godzilla.Abstractions.Collections;
using Godzilla.Abstractions.Infrastructure;
using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Collections.Internal
{
    internal class EntityNodesCollection : GodzillaCollection<EntityNode>,
        IEntityNodesCollection
    {
        /// <summary>
        /// For testing purpose only
        /// </summary>
        internal EntityNodesCollection()
            : base(null)
        { }

        public EntityNodesCollection(IDatabaseCollection<EntityNode> collection)
            : base(collection)
        { }

        public virtual EntityNode GetNode(Guid nodeId)
        {
            return _collection
                .AsQueryable()
                .FirstOrDefault(x => nodeId == x.EntityId);
        }

        public virtual IEnumerable<EntityNode> GetNodes(IEnumerable<Guid> nodesId)
        {
            return _collection
                .AsQueryable()
                .Where(x => nodesId.Contains(x.EntityId))
                .ToList();
        }

        public virtual bool ExistsAny(IEnumerable<Guid> nodesId)
        {
            return _collection
                .AsQueryable()
                .Any(x => nodesId.Contains(x.EntityId));
        }

        public virtual bool NodeExists(Guid nodeId)
        {
            return _collection
                .AsQueryable()
                .Any(x => x.EntityId == nodeId);
        }

        public virtual async Task DeleteNode(Guid nodeId)
        {
            await _collection.Delete(x => x.EntityId == nodeId);
        }

        public virtual async Task DeleteNodes(IEnumerable<Guid> nodesId)
        {
            await _collection.Delete(x => nodesId.Contains(x.EntityId));
        }

        public virtual IEnumerable<EntityNode> GetDescendants(EntityNode node)
        {
            var path = node.IdPath;
            return _collection
                .AsQueryable()
                .Where(x => x.IdPath.StartsWith(path))
                .ToList();
        }
    }
}
