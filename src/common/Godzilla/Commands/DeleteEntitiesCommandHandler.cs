using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
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

        public Task<Unit> Handle(DeleteEntitiesCommand<TContext> request, CancellationToken cancellationToken)
        {
            try
            {
                var entityType = _commandsHelper.GetEntityType(request.Entities);
                var nodesId = _commandsHelper.GetEntitiesId(request.Entities);

                _transactionService.StartTransaction();

                var entityCollection = _transactionService.GetCollection(entityType);
                var edgesCollection = _transactionService.GetCollection<TreeEdge, TreeEdgesCollection>();

                _commandsHelper.VerifyEntitiesExist(nodesId, edgesCollection);

                entityCollection.Delete(request.Entities);
                edgesCollection.DeleteNodes(nodesId);

                _transactionService.CommitTransaction();
                return Unit.Task;
            }
            catch (Exception e)
            {
                _transactionService.AbortTransaction();
                throw new EntitiesDeleteException("Entities delete failed", e);
            }
        }
    }
}
