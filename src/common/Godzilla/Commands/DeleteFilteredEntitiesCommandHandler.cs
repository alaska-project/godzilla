using Godzilla.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Commands
{
    internal class DeleteFilteredEntitiesCommandHandler<TContext, TEntity> : IRequestHandler<DeleteFilteredEntitiesCommand<TContext, TEntity>, Unit>
        where TContext : EntityContext
    {
        private readonly ICollectionService<TContext> _collectionService;
        private readonly IMediator _mediator;

        public DeleteFilteredEntitiesCommandHandler(
            IMediator mediator,
            ICollectionService<TContext> collectionService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _collectionService = collectionService ?? throw new ArgumentNullException(nameof(collectionService));
        }

        public async Task<Unit> Handle(DeleteFilteredEntitiesCommand<TContext, TEntity> request, CancellationToken cancellationToken)
        {
            var collection = _collectionService.GetCollection<TEntity>();

            var entitiesToDelete = await collection.GetItems(request.Filter);
            if (!entitiesToDelete.Any())
                return Unit.Value;

            var command = new DeleteEntitiesCommand<TContext>(entitiesToDelete.Cast<object>());

            await _mediator.Send(command);

            return Unit.Value;
        }
    }
}
