using Godzilla.Events.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Infrastructure
{
    public interface IEventQueue<TContext> : IEventQueue
        where TContext : EntityContext
    { }

    public interface IEventQueue
    {
        Task PublishEvent(EventData @event);
        IDisposable SubscribeEvent(string eventCategory, Action<EventData> callback);
    }
}
