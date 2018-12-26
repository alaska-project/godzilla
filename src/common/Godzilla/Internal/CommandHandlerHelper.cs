using Godzilla.Abstractions.Collections;
using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
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

        public CommandHandlerHelper(IEntityPropertyResolver<TContext> propertyResolver)
        {
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
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

        public void VerifyEntitiesExist(IEnumerable<object> entities, TreeEdgesCollection edgesCollection)
        {
            var nodesId = entities
                .Select(x => _propertyResolver.GetEntityId(x, false))
                .ToList();

            var existingNodesId = edgesCollection
                .AsQueryable()
                .Where(x => nodesId.Contains(x.NodeId))
                .Select(x => x.NodeId)
                .ToList();

            var missingNodesId = nodesId
                .Except(existingNodesId)
                .ToList();

            if (missingNodesId.Any())
                throw new EntitiesNotFoundException($"Entities not found {string.Join(", ", missingNodesId)}");
        }
    }
}
