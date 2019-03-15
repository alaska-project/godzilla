using Godzilla.Abstractions.Infrastructure;
using Godzilla.Events.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Events.Providers
{
    public class InMemoryEventQueueProvider<TContext> : IEventQueueProvider<TContext>
        where TContext : EntityContext
    {
        public InMemoryEventQueueProvider()
        {
        }

        public EventReceivedHandler OnEventReceived { get; set; }

        public Task PublishEvent(EventData @event)
        {
            if (OnEventReceived != null)
                OnEventReceived.Invoke(@event);

            return Task.FromResult(true);
        }
    }
}
