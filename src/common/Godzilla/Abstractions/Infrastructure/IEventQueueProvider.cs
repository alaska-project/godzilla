using Godzilla.Events.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Infrastructure
{
    public delegate Task EventReceivedHandler(EventData @event);

    public interface IEventQueueProvider<TContext> : IEventQueueProvider
        where TContext : EntityContext
    { }

    public interface IEventQueueProvider
    {
        Task PublishEvent(EventData @event);
        EventReceivedHandler OnEventReceived { get; set; }
    }
}
