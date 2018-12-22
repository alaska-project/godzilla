using Godzilla.Abstractions.Services;
using Godzilla.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Services
{
    internal class EntityCommandRunner<TContext> : 
        IEntityCommandRunner
        where TContext : EntityContext
    {
        private readonly IMediator _mediator;

        public EntityCommandRunner(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<TEntity> Add<TEntity>(TEntity entity)
        {
            return (TEntity)await _mediator.Send(new CreateEntityCommand<TContext>(Guid.Empty, entity));
        }
    }
}
