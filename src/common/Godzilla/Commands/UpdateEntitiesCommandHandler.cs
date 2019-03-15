using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using Godzilla.Internal;
using Godzilla.Notifications.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Commands
{
    internal class UpdateEntitiesCommandHandler<TContext> : IRequestHandler<UpdateEntitiesCommand<TContext>, IEnumerable<object>>
        where TContext : EntityContext
    {
        private readonly ITransactionService<TContext> _transactionService;
        private readonly IEntityCommandsHelper<TContext> _commandsHelper;

        public UpdateEntitiesCommandHandler(
            ITransactionService<TContext> transactionService,
            IEntityCommandsHelper<TContext> commandsHelper)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _commandsHelper = commandsHelper ?? throw new ArgumentNullException(nameof(commandsHelper));
        }

        public async Task<IEnumerable<object>> Handle(UpdateEntitiesCommand<TContext> request, CancellationToken cancellationToken)
        {
            try
            {
                var entityType = _commandsHelper.GetEntityType(request.Entities);

                _transactionService.StartTransaction();

                var entityCollection = _transactionService.GetCollection(entityType);
                var edgesCollection = _transactionService.GetCollection<EntityNode, EntityNodesCollection>();

                await _commandsHelper.VerifyEntities(request.Entities, edgesCollection, SecurityRight.Update);

                await entityCollection.Update(request.Entities);

                _transactionService.CommitTransaction();

                await NotifyEntitiesUpdate(request.Entities);

                return request.Entities;
            }
            catch (Exception e)
            {
                _transactionService.AbortTransaction();
                throw new EntitiesUpdateException("Entities update failed", e);
            }
        }

        private async Task NotifyEntitiesUpdate(IEnumerable<object> entities)
        {
            foreach (var entity in entities)
            {
                await _commandsHelper.PublishEntityEvent(new EntityUpdatedEvent
                {
                    EntityId = _commandsHelper.GetEntityId(entity),
                });
            }
        }
    }
}
