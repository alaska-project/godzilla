using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Queries
{
    internal class EntityQueries<TContext> : IEntityQueries
        where TContext : EntityContext
    {
        private readonly ICollectionService<TContext> _collectionService;
        private readonly IEntityPropertyResolver<TContext> _propertyResolver;

        public EntityQueries(ICollectionService<TContext> collectionService,
            IEntityPropertyResolver<TContext> propertyResolver)
        {
            _collectionService = collectionService ?? throw new ArgumentNullException(nameof(collectionService));
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
        }

        public IQueryable<TEntity> AsQueryable<TEntity>()
        {
            return GetCollection<TEntity>()
                .AsQueryable();
        }

        public IEnumerable<TChild> GetChildren<TChild>(object entity)
        {
            var parentId = _propertyResolver.GetEntityId(entity);
            var childrenCollection = GetCollection<TChild>();

            var childrenNodes = GetTreeEdgesCollection()
                .AsQueryable()
                .Where(x => 
                    x.ParentId == parentId && 
                    x.CollectionId == childrenCollection.CollectionId)
                .Select(x => x.NodeId)
                .ToList();

            return childrenCollection.GetItems(childrenNodes);
        }

        private TreeEdgesCollection GetTreeEdgesCollection()
        {
            return _collectionService.GetCollection<TreeEdge, TreeEdgesCollection>();
        }

        private IGodzillaCollection<TEntity> GetCollection<TEntity>()
        {
            var collection = _collectionService.GetCollection<TEntity>();
            if (collection == null)
                throw new CollectionNotFoundException($"No collections found for type {typeof(TEntity).FullName}");

            return collection;
        }
    }
}
