﻿using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using Godzilla.Notifications.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Commands
{
    internal class PartialUpdateEntitiesCommandHandler<TContext, TEntity, TField> : IRequestHandler<PartialUpdateEntitiesCommand<TContext, TEntity, TField>, IEnumerable<TEntity>>
        where TContext : EntityContext
    {
        private readonly ITransactionService<TContext> _transactionService;
        private readonly IEntityCommandsHelper<TContext> _commandsHelper;

        public PartialUpdateEntitiesCommandHandler(
            ITransactionService<TContext> transactionService,
            IEntityCommandsHelper<TContext> commandsHelper)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _commandsHelper = commandsHelper ?? throw new ArgumentNullException(nameof(commandsHelper));
        }

        public async Task<IEnumerable<TEntity>> Handle(PartialUpdateEntitiesCommand<TContext, TEntity, TField> request, CancellationToken cancellationToken)
        {
            try
            {
                _transactionService.StartTransaction();

                var entityCollection = _transactionService.GetCollection<TEntity>();
                var edgesCollection = _transactionService.GetCollection<EntityNode, EntityNodesCollection>();

                await _commandsHelper.VerifyEntities(request.Entities, edgesCollection, SecurityRight.Update);

                await entityCollection.Update<TField>(request.Entities, request.Field, request.Value);

                _transactionService.CommitTransaction();

                var entities = await entityCollection.GetItems(request.Entities);

                await NotifyEntitiesUpdate(entities);

                return entities;
            }
            catch (Exception e)
            {
                _transactionService.AbortTransaction();
                throw new EntitiesUpdateException("Entities update failed", e);
            }
        }

        private async Task NotifyEntitiesUpdate(IEnumerable<TEntity> entities)
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
