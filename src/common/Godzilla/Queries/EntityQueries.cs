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
using System.Threading.Tasks;

namespace Godzilla.Queries
{
    internal class EntityQueries<TContext> : IEntityQueries
        where TContext : EntityContext
    {
        private readonly ICollectionService<TContext> _collectionService;
        private readonly IEntityPropertyResolver<TContext> _propertyResolver;
        private readonly IPathBuilder<TContext> _pathBuilder;

        public EntityQueries(ICollectionService<TContext> collectionService,
            IEntityPropertyResolver<TContext> propertyResolver,
            IPathBuilder<TContext> pathBuilder)
        {
            _collectionService = collectionService ?? throw new ArgumentNullException(nameof(collectionService));
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
            _pathBuilder = pathBuilder ?? throw new ArgumentNullException(nameof(pathBuilder));
        }
        
        public IQueryable<TEntity> AsQueryable<TEntity>()
        {
            return GetCollection<TEntity>()
                .AsQueryable();
        }

        public async Task<TEntity> GetItem<TEntity>(string path)
        {
            return (await GetItems<TEntity>(path))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<TEntity>> GetItems<TEntity>(string path)
        {
            var normalizedPath = _pathBuilder.NormalizePath(path);
            var nodesId = GetTreeEdgesCollection()
                .AsQueryable()
                .Where(x => x.Path == normalizedPath)
                .Select(x => x.EntityId)
                .ToList();

            return await GetCollection<TEntity>()
                .GetItems(nodesId);
        }

        public async Task<TEntity> GetItem<TEntity>(Guid id)
        {
            return await GetCollection<TEntity>()
                .GetItem(id);
        }

        public async Task<TItem> GetItem<TItem>(Expression<Func<TItem, bool>> filter)
        {
            return await Task.FromResult(GetCollection<TItem>()
                .AsQueryable()
                .Where(filter)
                .FirstOrDefault());
        }
        
        public async Task<IEnumerable<TEntity>> GetItems<TEntity>(IEnumerable<Guid> id)
        {
            return await GetCollection<TEntity>()
                .GetItems(id);
        }

        public Task<IEnumerable<TItem>> GetItems<TItem>(Expression<Func<TItem, bool>> filter)
        {
            return Task.FromResult<IEnumerable<TItem>>(GetCollection<TItem>()
                .AsQueryable()
                .Where(filter)
                .ToList());
        }

        public async Task<TParent> GetParent<TParent>(object entity)
        {
            var itemId = _propertyResolver.GetEntityId(entity);
            var parentCollection = GetCollection<TParent>();

            var itemNode = GetTreeEdgesCollection()
                .AsQueryable()
                .FirstOrDefault(x =>
                    x.EntityId == itemId &&
                    x.CollectionId == parentCollection.CollectionId);

            if (itemNode == null || itemNode.ParentId == Guid.Empty)
                return default(TParent);

            return await parentCollection.GetItem(itemNode.ParentId);
        }

        public async Task<TChild> GetChild<TChild>(object entity)
        {
            return (await GetChildren<TChild>(entity))
                .FirstOrDefault();
        }

        public async Task<TChild> GetChild<TChild>(object entity, Expression<Func<TChild, bool>> filter)
        {
            return (await GetChildren<TChild>(entity, filter))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<TChild>> GetChildren<TChild>(object entity)
        {
            return await GetChildren<TChild>(entity, null);
        }

        public async Task<IEnumerable<TChild>> GetChildren<TChild>(object entity, Expression<Func<TChild, bool>> filter)
        {
            var parentId = _propertyResolver.GetEntityId(entity);
            var childrenCollection = GetCollection<TChild>();

            var childrenNodes = GetTreeEdgesCollection()
                .AsQueryable()
                .Where(x =>
                    x.ParentId == parentId &&
                    x.CollectionId == childrenCollection.CollectionId)
                .Select(x => x.EntityId)
                .ToList();

            return await (filter == null ? 
                childrenCollection.GetItems(childrenNodes) :
                childrenCollection.GetItems(childrenNodes, filter));
        }

        private EntityNodesCollection GetTreeEdgesCollection()
        {
            return _collectionService.GetCollection<EntityNode, EntityNodesCollection>();
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
