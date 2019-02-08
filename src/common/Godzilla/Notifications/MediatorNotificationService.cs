using Godzilla.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Notifications
{
    internal class MediatorNotificationService<TContext> : INotificationService<TContext>
        where TContext : EntityContext
    {
        private IMediator _mediator;

        public MediatorNotificationService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishEvent(object @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.Publish(@event, cancellationToken);
        }
    }
}
