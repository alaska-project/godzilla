﻿using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using Godzilla.Notifications.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Commands
{
    internal class CreateEntitiesCommandHandler<TContext> : IRequestHandler<CreateEntitiesCommand<TContext>, IEnumerable<object>>
        where TContext : EntityContext
    {
        private readonly ITransactionService<TContext> _transactionService;
        private readonly IEntityPropertyResolver<TContext> _propertyResolver;
        private readonly IEntityCommandsHelper<TContext> _commandsHelper;

        public CreateEntitiesCommandHandler(
            ITransactionService<TContext> transactionService,
            IEntityPropertyResolver<TContext> propertyResolver,
            IEntityCommandsHelper<TContext> commandsHelper)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
            _commandsHelper = commandsHelper ?? throw new ArgumentNullException(nameof(commandsHelper));
        }

        public async Task<IEnumerable<object>> Handle(CreateEntitiesCommand<TContext> request, CancellationToken cancellationToken)
        {
            try
            {
                var entityType = _commandsHelper.GetEntityType(request.Entities);
                
                _transactionService.StartTransaction();

                var entityCollection = _transactionService.GetCollection(entityType);
                var edgesCollection = _transactionService.GetCollection<EntityNode, EntityNodesCollection>();

                var parent = await GetParentNode(edgesCollection, request.ParentId);

                var treeEdges = request.Entities
                    .Select(x => CreateTreeEdge(x, parent, entityCollection))
                    .ToList();
                
                ValidateTreeEdges(edgesCollection, treeEdges);

                await edgesCollection.Add(treeEdges);
                
                await entityCollection.Add(request.Entities);

                _transactionService.CommitTransaction();

                await NotifyEntitiesCreation(request.Entities, parent);

                return request.Entities;
            }
            catch (Exception e)
            {
                _transactionService.AbortTransaction();
                throw new EntitiesCreationException("Entities creation failed", e);
            }
        }

        private async Task<EntityNode> GetParentNode(EntityNodesCollection edgesCollection, Guid parentId)
        {
            if (parentId == Guid.Empty)
            {
                await _commandsHelper.VerifyRootNodePermission(SecurityRight.Create);
                return null;
            }

            var parent = await _commandsHelper.VerifyEntity(parentId, edgesCollection, SecurityRight.Create);
            if (parent == null)
                throw new ParentNodeNotFoundException($"Parent node {parentId} not found");

            return parent;
        }

        private void ValidateTreeEdges(EntityNodesCollection edgesCollection, IEnumerable<EntityNode> treeEdges)
        {
            var newNodesId = treeEdges
                .Select(x => x.EntityId)
                .ToList();

            var existingNodes = edgesCollection.GetNodes(newNodesId);
            if (existingNodes.Any())
                throw new NodeAlreadyExistsException($"Node {string.Join(", ", existingNodes.Select(x => x.EntityId))} already exists");
        }

        private async Task NotifyEntitiesCreation(IEnumerable<object> entities, EntityNode parent)
        {
            foreach (var entity in entities)
            {
                await _commandsHelper.PublishEntityEvent(new EntityCreatedEvent
                {
                    EntityId = _commandsHelper.GetEntityId(entity),
                    ParentId = parent?.EntityId ?? Guid.Empty,
                });
            }
        }

        private EntityNode CreateTreeEdge(object entity, EntityNode parent, IGodzillaCollection entityCollection)
        {
            var entityId = _propertyResolver.GetEntityId(entity, true);
            var entityName = _propertyResolver.GetEntityName(entity);

            return new EntityNode
            {
                Id = Guid.NewGuid(),
                EntityId = entityId,
                NodeName = entityName,
                ParentId = parent?.EntityId ?? Guid.Empty,
                CollectionId = entityCollection.CollectionId,
                Path = _commandsHelper.BuildNamePath(entityName, parent),
                IdPath = _commandsHelper.BuildIdPath(entityId, parent),
            };
        }
    }
}
