using Godzilla.Abstractions.Services;
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
    internal class DeleteEntitiesCommandHandler<TContext> : IRequestHandler<DeleteEntitiesCommand<TContext>, Unit>
        where TContext : EntityContext
    {
        private readonly ITransactionService<TContext> _transactionService;
        private readonly IEntityCommandsHelper<TContext> _commandsHelper;

        public DeleteEntitiesCommandHandler(
            ITransactionService<TContext> transactionService,
            IEntityCommandsHelper<TContext> commandsHelper)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _commandsHelper = commandsHelper ?? throw new ArgumentNullException(nameof(commandsHelper));
        }

        public async Task<Unit> Handle(DeleteEntitiesCommand<TContext> request, CancellationToken cancellationToken)
        {
            try
            {
                var entityType = _commandsHelper.GetEntityType(request.Entities);
                var nodesId = _commandsHelper.GetEntitiesId(request.Entities);

                _transactionService.StartTransaction();
                
                var edgesCollection = _transactionService.GetCollection<EntityNode, EntityNodesCollection>();

                var treeNodes = await _commandsHelper.VerifyEntities(nodesId, edgesCollection, SecurityRight.Delete);
                var deletedNodes = new List<EntityNode>();

                foreach (var treeNode in treeNodes)
                {
                    deletedNodes.AddRange(await DeleteEntityAndDescendants(treeNode, edgesCollection));
                }
                
                _transactionService.CommitTransaction();

                await NotifyEntitiesDeletion(deletedNodes);

                return Unit.Value;
            }
            catch (Exception e)
            {
                _transactionService.AbortTransaction();
                throw new EntitiesDeleteException("Entities delete failed", e);
            }
        }

        private async Task<IEnumerable<EntityNode>> DeleteEntityAndDescendants(EntityNode deleteRoot, EntityNodesCollection edgesCollection)
        {
            var descendants = edgesCollection.GetDescendants(deleteRoot);

            var groupedEntities = descendants.GroupBy(x => x.CollectionId);

            foreach (var entityGroup in groupedEntities)
            {
                var entitiesIdToDelete = entityGroup
                    .Select(x => x.EntityId)
                    .ToList();

                var entityCollection = _transactionService.GetCollection(typeof(object), entityGroup.Key);
                await entityCollection.Delete(entitiesIdToDelete);
            }

            await edgesCollection.DeleteNodes(descendants.Select(x => x.EntityId));

            return descendants;
        }

        private async Task NotifyEntitiesDeletion(IEnumerable<EntityNode> nodes)
        {
            foreach (var node in nodes)
            {
                await _commandsHelper.PublishEntityEvent(new EntityDeletedEvent
                {
                    EntityId = node.EntityId,
                    ParentId = node.ParentId,
                });
            }
        }
    }
}
