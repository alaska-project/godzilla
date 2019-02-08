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
        private readonly ISecurityRuleEvaluator<TContext> _securityEvaluator;

        public EntityQueries(ICollectionService<TContext> collectionService,
            IEntityPropertyResolver<TContext> propertyResolver,
            IPathBuilder<TContext> pathBuilder,
            ISecurityRuleEvaluator<TContext> securityEvaluator)
        {
            _collectionService = collectionService ?? throw new ArgumentNullException(nameof(collectionService));
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
            _pathBuilder = pathBuilder ?? throw new ArgumentNullException(nameof(pathBuilder));
            _securityEvaluator = securityEvaluator ?? throw new ArgumentNullException(nameof(securityEvaluator));
        }
        
        //public IQueryable<TEntity> AsQueryable<TEntity>()
        //{
        //    return GetCollection<TEntity>()
        //        .AsQueryable();
        //}

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

            var filteredNodes = await FilterAllowedNodes(nodesId);

            return await GetCollection<TEntity>()
                .GetItems(filteredNodes);
        }

        public async Task<TEntity> GetItem<TEntity>(Guid id)
        {
            var filteredNodes = await FilterAllowedNodes(new List<Guid> { id });
            if (!filteredNodes.Any())
                return default(TEntity);

            var filteredNode = filteredNodes.FirstOrDefault();
            return await GetCollection<TEntity>()
                .GetItem(filteredNode);
        }

        public async Task<TItem> GetItem<TItem>(Expression<Func<TItem, bool>> filter)
        {
            var results = await GetItems<TItem>(filter);
            return results.FirstOrDefault();
        }
        
        public async Task<IEnumerable<TEntity>> GetItems<TEntity>(IEnumerable<Guid> id)
        {
            var filteredNodes = await FilterAllowedNodes(id);

            return await GetCollection<TEntity>()
                .GetItems(filteredNodes);
        }

        public async Task<IEnumerable<TItem>> GetItems<TItem>(Expression<Func<TItem, bool>> filter)
        {
            var entities = GetCollection<TItem>()
                .AsQueryable()
                .Where(filter)
                .ToList();

            var filteredNodes = await FilterAllowedNodes(entities);
            return filteredNodes;
        }

        public async Task<TParent> GetParent<TParent>(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var itemId = _propertyResolver.GetEntityId(entity);
            var parentCollection = GetCollection<TParent>();

            var itemNode = GetTreeEdgesCollection()
                .AsQueryable()
                .FirstOrDefault(x =>
                    x.EntityId == itemId &&
                    x.CollectionId == parentCollection.CollectionId);

            if (itemNode == null || itemNode.ParentId == Guid.Empty)
                return default(TParent);

            var filteredNodes = await FilterAllowedNodes(new List<Guid> { itemNode.ParentId });
            if (!filteredNodes.Any())
                return default(TParent);

            return await parentCollection.GetItem(filteredNodes.First());
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

        public async Task<TChild> GetChild<TChild>(object entity, Guid entityId)
        {
            return (await GetChildren<TChild>(entity, new List<Guid> { entityId }))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<TChild>> GetChildren<TChild>(object entity)
        {
            return await GetChildren<TChild>(entity, (Expression<Func<TChild, bool>>)null);
        }

        public async Task<IEnumerable<TChild>> GetChildren<TChild>(object entity, IEnumerable<Guid> id)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var parentId = _propertyResolver.GetEntityId(entity);
            var childrenCollection = GetCollection<TChild>();

            var childrenNodes = GetTreeEdgesCollection()
                .AsQueryable()
                .Where(x =>
                    id.Contains(x.EntityId) &&
                    x.ParentId == parentId &&
                    x.CollectionId == childrenCollection.CollectionId)
                .Select(x => x.EntityId)
                .ToList();

            var filteredChildNodes = await FilterAllowedNodes(childrenNodes);
            return await childrenCollection.GetItems(filteredChildNodes);
        }

        public async Task<IEnumerable<TChild>> GetChildren<TChild>(object entity, Expression<Func<TChild, bool>> filter)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var parentId = _propertyResolver.GetEntityId(entity);
            var childrenCollection = GetCollection<TChild>();

            var childrenNodes = GetTreeEdgesCollection()
                .AsQueryable()
                .Where(x =>
                    x.ParentId == parentId &&
                    x.CollectionId == childrenCollection.CollectionId)
                .Select(x => x.EntityId)
                .ToList();

            var filteredChildNodes = await FilterAllowedNodes(childrenNodes);

            return await (filter == null ? 
                childrenCollection.GetItems(filteredChildNodes) :
                childrenCollection.GetItems(filteredChildNodes, filter));
        }

        private async Task<IEnumerable<TEntity>> FilterAllowedNodes<TEntity>(IEnumerable<TEntity> entities)
        {
            var entitiesId = entities
                .Select(x => _propertyResolver.GetEntityId(x))
                .ToList();

            var allowedEntitiesId = await FilterAllowedNodes(entitiesId);

            return entities
                .Where(x => allowedEntitiesId.Contains(_propertyResolver.GetEntityId(x)))
                .ToList();
        }

        private async Task<IEnumerable<Guid>> FilterAllowedNodes(IEnumerable<Guid> nodesId)
        {
            var evaluateResults = await _securityEvaluator.Evaluate(nodesId, SecurityRight.Read);

            return evaluateResults
                .Where(x => x.IsRightGranted)
                .Select(x => x.EntityId)
                .ToList();
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
