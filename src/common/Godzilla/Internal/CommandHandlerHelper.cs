using Godzilla.Abstractions;
using Godzilla.Abstractions.Collections;
using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Internal
{
    internal class CommandHandlerHelper<TContext> : IEntityCommandsHelper<TContext>
        where TContext : EntityContext
    {
        private readonly IEntityPropertyResolver<TContext> _propertyResolver;
        private readonly IPathBuilder<TContext> _pathBuilder;
        private readonly ISecurityRuleEvaluator<TContext> _securityEvaluator;
        private readonly IEntityNotificationService<TContext> _notificationService;

        public CommandHandlerHelper(
            IEntityPropertyResolver<TContext> propertyResolver,
            IPathBuilder<TContext> pathBuilder,
            ISecurityRuleEvaluator<TContext> securityEvaluator,
            IEntityNotificationService<TContext> notificationService)
        {
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
            _pathBuilder = pathBuilder ?? throw new ArgumentNullException(nameof(pathBuilder));
            _securityEvaluator = securityEvaluator ?? throw new ArgumentNullException(nameof(securityEvaluator));
            _notificationService = notificationService;
        }

        public async Task PublishEntityEvent<TEvent>(TEvent eventData)
        {
            await _notificationService.PublishEntityEvent(eventData);
        }

        public Type GetEntityType(IEnumerable<object> entities)
        {
            var entityTypes = entities
                .Where(x => x != null)
                .GroupBy(x => x.GetType());

            if (entityTypes.Count() > 1)
                throw new MultipleEntityTypesNotSupportedException($"Cannot add multiple entity types inside the same operation. Types: {string.Join(", ", entityTypes.Select(x => x.Key.FullName))}");

            return entityTypes.First().Key;
        }

        public async Task<EntityNode> VerifyEntity(Guid entitiyId, EntityNodesCollection edgesCollection, Guid permission)
        {
            var entities = await VerifyEntities(new List<Guid> { entitiyId }, edgesCollection, permission);
            return entities.FirstOrDefault();
        }

        public async Task<IEnumerable<EntityNode>> VerifyEntities<TEntity>(IEnumerable<TEntity> entities, EntityNodesCollection edgesCollection, Guid permission)
        {
            return await VerifyEntities(entities.Cast<object>(), edgesCollection, permission);
        }

        public async Task<IEnumerable<EntityNode>> VerifyEntities(IEnumerable<object> entities, EntityNodesCollection edgesCollection, Guid permission)
        {
            var entitiesId = GetEntitiesId(entities);

            return await VerifyEntities(entitiesId, edgesCollection, permission);
        }
        
        public async Task<IEnumerable<EntityNode>> VerifyEntities(IEnumerable<Guid> entitiesId, EntityNodesCollection edgesCollection, Guid permission)
        {
            var existingNodes = edgesCollection
                .AsQueryable()
                .Where(x => entitiesId.Contains(x.EntityId))
                .ToList();

            var existingNodesId = existingNodes
                .Select(x => x.EntityId);

            var missingNodesId = entitiesId
                .Except(existingNodesId)
                .ToList();

            if (missingNodesId.Any())
                throw new EntitiesNotFoundException($"Entities not found {string.Join(", ", missingNodesId)}");

            await VerifyEntitiesAuthorization(entitiesId, permission);

            return existingNodes;
        }

        public async Task VerifyRootNodePermission(Guid permission)
        {
            var result = await _securityEvaluator.EvaluateRoot(permission);
            if (!result.IsRightGranted)
                throw new UnauthorizedOperationException($"Unauthorized access right {permission} for root entity");
        }
        
        public IEnumerable<Guid> GetEntitiesId<TEntity>(IEnumerable<TEntity> entities)
        {
            return GetEntitiesId(entities.Cast<object>());
        }

        public IEnumerable<Guid> GetEntitiesId(IEnumerable<object> entities)
        {
            return entities
                .Select(x => GetEntityId(x))
                .ToList();
        }

        public Guid GetEntityId(object entity)
        {
            return _propertyResolver.GetEntityId(entity, false);
        }

        public string BuildNamePath(string name, EntityNode parent)
        {
            var parentPath = parent?.Path ?? _pathBuilder.RootPath;
            return _pathBuilder.JoinPath(parentPath, name);
        }

        public string BuildIdPath(Guid id, EntityNode parent)
        {
            var parentPath = parent?.IdPath ?? _pathBuilder.RootPath;
            return _pathBuilder.JoinPath(parentPath, id.ToString());
        }

        private async Task VerifyEntitiesAuthorization(IEnumerable<Guid> entitiesId, Guid permission)
        {
            var evaluateResults = await _securityEvaluator.Evaluate(entitiesId, permission);
            var unauthorizedEntities = evaluateResults
                .Where(x => !x.IsRightGranted)
                .ToList();

            if (unauthorizedEntities.Any())
                throw new UnauthorizedOperationException($"Unauthorized access right {permission} for entities {string.Join(", ", unauthorizedEntities.Select(x => x.EntityId))}");
        }
    }
}
