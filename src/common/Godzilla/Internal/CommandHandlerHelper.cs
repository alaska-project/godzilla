using Godzilla.Abstractions.Collections;
using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Internal
{
    internal class CommandHandlerHelper<TContext> : IEntityCommandsHelper<TContext>
        where TContext : EntityContext
    {
        private readonly IEntityPropertyResolver<TContext> _propertyResolver;
        private readonly IPathBuilder<TContext> _pathBuilder;

        public CommandHandlerHelper(
            IEntityPropertyResolver<TContext> propertyResolver,
            IPathBuilder<TContext> pathBuilder)
        {
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
            _pathBuilder = pathBuilder ?? throw new ArgumentNullException(nameof(pathBuilder));
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

        public IEnumerable<TreeEdge> VerifyEntitiesExist<TEntity>(IEnumerable<TEntity> entities, TreeEdgesCollection edgesCollection)
        {
            return VerifyEntitiesExist(entities.Cast<object>(), edgesCollection);
        }

        public IEnumerable<TreeEdge> VerifyEntitiesExist(IEnumerable<object> entities, TreeEdgesCollection edgesCollection)
        {
            var entitiesId = GetEntitiesId(entities);

            return VerifyEntitiesExist(entitiesId, edgesCollection);
        }

        public IEnumerable<TreeEdge> VerifyEntitiesExist(IEnumerable<Guid> entitiesId, TreeEdgesCollection edgesCollection)
        {
            var existingNodes = edgesCollection
                .AsQueryable()
                .Where(x => entitiesId.Contains(x.NodeId))
                .ToList();

            var existingNodesId = existingNodes
                .Select(x => x.NodeId);

            var missingNodesId = entitiesId
                .Except(existingNodesId)
                .ToList();

            if (missingNodesId.Any())
                throw new EntitiesNotFoundException($"Entities not found {string.Join(", ", missingNodesId)}");

            return existingNodes;
        }

        public IEnumerable<Guid> GetEntitiesId<TEntity>(IEnumerable<TEntity> entities)
        {
            return GetEntitiesId(entities.Cast<object>());
        }

        public IEnumerable<Guid> GetEntitiesId(IEnumerable<object> entities)
        {
            return entities
                .Select(x => _propertyResolver.GetEntityId(x, false))
                .ToList();
        }

        public string BuildNamePath(string name, TreeEdge parent)
        {
            var parentPath = parent?.Path ?? _pathBuilder.RootPath;
            return _pathBuilder.JoinPath(parentPath, name);
        }

        public string BuildIdPath(Guid id, TreeEdge parent)
        {
            var parentPath = parent?.IdPath ?? _pathBuilder.RootPath;
            return _pathBuilder.JoinPath(parentPath, id.ToString());
        }
    }
}
