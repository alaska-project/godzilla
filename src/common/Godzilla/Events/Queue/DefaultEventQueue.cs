using Godzilla.Abstractions.Infrastructure;
using Godzilla.Events.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Events.Queue
{
    public class DefaultEventQueue<TContext> : IEventQueue<TContext>
        where TContext : EntityContext
    {
        private readonly IEventQueueProvider<TContext> _eventQueueProvider;
        private readonly Dictionary<string, List<EventQueueSubscription<TContext>>> _subscriptions = new Dictionary<string, List<EventQueueSubscription<TContext>>>();

        public DefaultEventQueue(IEventQueueProvider<TContext> eventQueueProvider)
        {
            _eventQueueProvider = eventQueueProvider;
            _eventQueueProvider.OnEventReceived += ProcessEvent;
        }

        public async Task PublishEvent(EventData @event)
        {
            await _eventQueueProvider.PublishEvent(@event);
        }

        private Task ProcessEvent(EventData @event)
        {
            if (!_subscriptions.ContainsKey(@event.EventCategory))
                return Task.FromResult(false);

            var subscriptions = _subscriptions[@event.EventCategory];
            foreach (var subscription in subscriptions)
                InvokeCallback(subscription, @event);

            return Task.FromResult(false);
        }

        private void InvokeCallback(EventQueueSubscription<TContext> subscription, EventData @event)
        {
            Task.Run(() => subscription.Callback(@event)).ConfigureAwait(false);
        }

        public IDisposable SubscribeEvent(string eventCategory, Action<EventData> callback)
        {
            var subscription = new EventQueueSubscription<TContext>(this, eventCategory, callback);

            lock (this)
            {
                if (!_subscriptions.ContainsKey(eventCategory))
                    _subscriptions.Add(eventCategory, new List<EventQueueSubscription<TContext>>());
                _subscriptions[eventCategory].Add(subscription);
            }

            return subscription;
        }

        internal void Unsubscribe(EventQueueSubscription<TContext> subscription)
        {
            lock (this)
            {
                if (!_subscriptions.ContainsKey(subscription.EventCategory))
                    throw new InvalidOperationException($"Event category subscriptions {subscription.EventCategory} not found");

                var subscriptions = _subscriptions[subscription.EventCategory];
                if (!subscriptions.Contains(subscription))
                    throw new InvalidOperationException($"Subscription not found");

                subscriptions.Remove(subscription);
            }
        }
    }
}
