//using Godzilla.Abstractions;
//using Godzilla.DomainModels;
//using Godzilla.Exceptions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Godzilla.Collections
//{
//    internal class TreeEdgesCollection<TContext>
//        where TContext : EntityContext
//    {
//        private readonly IDatabaseCollection<TreeEdge> _edgeCollection;
        
//        public TreeEdgesCollection(
//            IGodzillaOptions<TContext> options,
//            IDatabaseCollectionProvider<TContext> builder)
//        {
//            if (options == null)
//                throw new ArgumentNullException(nameof(options));

//            if (builder == null)
//                throw new ArgumentNullException(nameof(builder));

//            _edgeCollection = builder.GetCollection<TreeEdge>(options.TreeEdgesCollectionId);
//        }

//        public void AddEdge(TreeEdge edge)
//        {
//            if (_edgeCollection.AsQueryable()
//                .Any(x => x.NodeId == edge.NodeId))
//                throw new NodeAlreadyExistsException($"Node {edge.NodeId} already exists");

//            _edgeCollection.Add(edge);
//        }

//        public bool NodeExists(Guid nodeId)
//        {
//            return _edgeCollection.AsQueryable()
//                .Any(x => x.NodeId == nodeId);
//        }
//    }
//}
