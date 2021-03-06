﻿using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
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
        private readonly IEntityCollectionsService<TContext> _collectionService;
        private readonly IEntityPropertyResolver<TContext> _propertyResolver;
        private readonly IPathBuilder<TContext> _pathBuilder;
        private readonly ISecurityRuleEvaluator<TContext> _securityEvaluator;

        public EntityQueries(IEntityCollectionsService<TContext> collectionService,
            IEntityPropertyResolver<TContext> propertyResolver,
            IPathBuilder<TContext> pathBuilder,
            ISecurityRuleEvaluator<TContext> securityEvaluator)
        {
            _collectionService = collectionService ?? throw new ArgumentNullException(nameof(collectionService));
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
            _pathBuilder = pathBuilder ?? throw new ArgumentNullException(nameof(pathBuilder));
            _securityEvaluator = securityEvaluator ?? throw new ArgumentNullException(nameof(securityEvaluator));
        }

        public IQueryable<TEntity> AsQueryable<TEntity>()
        {
            if (_securityEvaluator.IsAuthEnabled())
                throw new NotSupportedException($"AsQueryable not allowed for contexts with security authentication enabled. You can explicitly disable outhentication for scope using {nameof(ISecurityDisablerService)}");

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
            var nodesId = GetEntityNodesCollection()
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

            var nodesCollection = GetEntityNodesCollection();
            var itemId = _propertyResolver.GetEntityId(entity);
            var itemNode = nodesCollection.GetNode(itemId);

            var parentEntityId = itemNode.ParentId;
            var parentCollection = GetCollection<TParent>();
            var parentCollectionId = parentCollection.CollectionId;

            var parentNode = nodesCollection
                .AsQueryable()
                .FirstOrDefault(x =>
                    x.EntityId == parentEntityId &&
                    x.CollectionId == parentCollectionId);

            if (itemNode == null || itemNode.ParentId == Guid.Empty)
                return default(TParent);

            var filteredNodes = await FilterAllowedNodes(new List<Guid> { itemNode.ParentId });
            if (!filteredNodes.Any())
                return default(TParent);

            return await parentCollection.GetItem(filteredNodes.First());
        }

        public async Task<TChild> GetChild<TChild>(object entity, string name)
        {
            return (await GetChildren<TChild>(entity, name))
                .FirstOrDefault();
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
            var childrenCollectionId = childrenCollection.CollectionId;

            var childrenNodes = GetEntityNodesCollection()
                .AsQueryable()
                .Where(x =>
                    id.Contains(x.EntityId) &&
                    x.ParentId == parentId &&
                    x.CollectionId == childrenCollectionId)
                .Select(x => x.EntityId)
                .ToList();

            var filteredChildNodes = await FilterAllowedNodes(childrenNodes);
            return await childrenCollection.GetItems(filteredChildNodes);
        }

        public async Task<IEnumerable<TChild>> GetChildren<TChild>(object entity, string name)
        {
            return await GetChildren<TChild>(entity, null, name);
        }

        public async Task<IEnumerable<TChild>> GetChildren<TChild>(object entity, Expression<Func<TChild, bool>> filter)
        {
            return await GetChildren<TChild>(entity, filter, null);
        }

        public async Task<IEnumerable<TChild>> GetChildren<TChild>(object entity, Expression<Func<TChild, bool>> filter, string name)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var parentId = _propertyResolver.GetEntityId(entity);
            var childrenCollection = GetCollection<TChild>();
            var childrenCollectionId = childrenCollection.CollectionId;

            var queryableEntityNodes = GetEntityNodesCollection()
                .AsQueryable()
                .Where(x =>
                    x.ParentId == parentId &&
                    x.CollectionId == childrenCollectionId);

            if (!string.IsNullOrEmpty(name))
                queryableEntityNodes = queryableEntityNodes.Where(x => x.NodeName.ToLower() == name.ToLower());

            var childrenNodes = queryableEntityNodes
                .Select(x => x.EntityId)
                .ToList();

            var filteredChildNodes = await FilterAllowedNodes(childrenNodes);

            return await (filter == null ? 
                childrenCollection.GetItems(filteredChildNodes) :
                childrenCollection.GetItems(filteredChildNodes, filter));
        }

        public async Task<long> GetChildrenCount<TChild>(object entity)
        {
            var childrenCollection = GetCollection<TChild>();
            var childrenCollectionId = childrenCollection.CollectionId;
            return await GetChildrenCount(entity, childrenCollectionId);
        }

        public async Task<long> GetChildrenCount(object entity)
        {
            return await GetChildrenCount(entity, null);
        }

        private async Task<long> GetChildrenCount(object entity, string childrenCollectionId)
        {
            var parentId = _propertyResolver.GetEntityId(entity);
            
            var queryableEntityNodes = GetEntityNodesCollection()
                .AsQueryable()
                .Where(x => x.ParentId == parentId);

            if (!string.IsNullOrEmpty(childrenCollectionId))
                queryableEntityNodes = queryableEntityNodes
                    .Where(x => x.CollectionId == childrenCollectionId);

            return await Task.FromResult(queryableEntityNodes.LongCount());
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

        internal EntityNodesCollection GetEntityNodesCollection()
        {
            return _collectionService.GetEntityNodesCollection();
        }

        internal IGodzillaCollection<TEntity> GetCollection<TEntity>()
        {
            return _collectionService.GetCollection<TEntity>();
        }
    }
}
