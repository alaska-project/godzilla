using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Commands
{
    public class CreateEntityCommandHandler<TContext> : IRequestHandler<CreateEntityCommand<TContext>, bool>
        where TContext : EntityContext
    {
        private readonly ITransactionService<TContext> _transactionService;
        private readonly IEntityPropertyResolver<TContext> _propertyResolver;

        internal CreateEntityCommandHandler(
            ITransactionService<TContext> transactionService,
            IEntityPropertyResolver<TContext> propertyResolver)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
        }

        public Task<bool> Handle(CreateEntityCommand<TContext> request, CancellationToken cancellationToken)
        {
            try
            {
                _transactionService.StartTransaction();

                var entityId = GetEntityId(request.Entity);

                var edgesCollection = _transactionService.GetCollection<TreeEdge, TreeEdgesCollection>();
                if (edgesCollection.NodeExists(entityId))
                    throw new NodeAlreadyExistsException($"Node {entityId} already exists");

                

                _transactionService.CommitTransaction();

                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                _transactionService.AbortTransaction();
                throw new EntityCreationException("Entity creation failed", e);
            }
        }

        private Guid GetEntityId(object entity)
        {
            return _propertyResolver.GetEntityId(entity, true);
        }

        private string GetEntityName(object entity)
        {
            return _propertyResolver.GetEntityName(entity);
        }
    }
}
