using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using Godzilla.Internal;
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

        public Task<IEnumerable<object>> Handle(UpdateEntitiesCommand<TContext> request, CancellationToken cancellationToken)
        {
            try
            {
                var entityType = _commandsHelper.GetEntityType(request.Entities);

                _transactionService.StartTransaction();

                var entityCollection = _transactionService.GetCollection(entityType);
                var edgesCollection = _transactionService.GetCollection<TreeEdge, TreeEdgesCollection>();

                _commandsHelper.VerifyEntitiesExist(request.Entities, edgesCollection);

                entityCollection.Update(request.Entities);

                _transactionService.CommitTransaction();
                return Task.FromResult(request.Entities);
            }
            catch (Exception e)
            {
                _transactionService.AbortTransaction();
                throw new EntitiesUpdateException("Entities update failed", e);
            }
        }
    }
}
