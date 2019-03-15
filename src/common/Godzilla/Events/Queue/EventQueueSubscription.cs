using Godzilla.Events.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Events.Queue
{
    internal class EventQueueSubscription<TContext> : IDisposable
        where TContext : EntityContext
    {
        private readonly DefaultEventQueue<TContext> _queue;

        public EventQueueSubscription(
            DefaultEventQueue<TContext> queue,
            string eventCategory,
            Action<EventData> callback)
        {
            this._queue = queue;
            EventCategory = eventCategory;
            Callback = callback;
        }

        public string EventCategory { get; }
        public Action<EventData> Callback { get; }

        public void Dispose()
        {
            _queue.Unsubscribe(this);
        }
    }
}
