using Godzilla.Abstractions.Services;
using Godzilla.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Commands
{
    public class CreateEntityCommandHandler<TContext> : IRequestHandler<CreateEntityCommand<TContext>, bool>
        where TContext : EntityContext
    {
        private readonly ITransactionService<TContext> _transactionService;

        public CreateEntityCommandHandler(
            ITransactionService<TContext> transactionService)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }

        public Task<bool> Handle(CreateEntityCommand<TContext> request, CancellationToken cancellationToken)
        {
            try
            {
                _transactionService.StartTransaction();

                _transactionService.CommitTransaction();

                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                _transactionService.AbortTransaction();
                throw new EntityCreationException("Entity creation failed", e);
            }
        }
    }
}
