using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public TEntity GetItem<TEntity>(Guid id)
        {
            return GetCollection<TEntity>()
                .GetItem(id);
        }

        public IEnumerable<TEntity> GetItems<TEntity>(IEnumerable<Guid> id)
        {
            return GetCollection<TEntity>()
                .GetItems(id);
        }

        public TParent GetParent<TParent>(object entity)
        {
            var itemId = _propertyResolver.GetEntityId(entity);
            var parentCollection = GetCollection<TParent>();

            var itemNode = GetTreeEdgesCollection()
                .AsQueryable()
                .FirstOrDefault(x =>
                    x.NodeId == itemId &&
                    x.CollectionId == parentCollection.CollectionId);

            if (itemNode == null || itemNode.ParentId == Guid.Empty)
                return default(TParent);

            return parentCollection.GetItem(itemNode.ParentId);
        }

        public TChild GetChild<TChild>(object entity)
        {
            return GetChildren<TChild>(entity)
                .FirstOrDefault();
        }

        public TChild GetChild<TChild>(object entity, Expression<Func<TChild, bool>> filter)
        {
            return GetChildren<TChild>(entity, filter)
                .FirstOrDefault();
        }

        public IEnumerable<TChild> GetChildren<TChild>(object entity)
        {
            return GetChildren<TChild>(entity, null);
        }

        public IEnumerable<TChild> GetChildren<TChild>(object entity, Expression<Func<TChild, bool>> filter)
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

            return filter == null ? 
                childrenCollection.GetItems(childrenNodes) :
                childrenCollection.GetItems(childrenNodes, filter);
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
