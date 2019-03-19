using Godzilla.Abstractions.Infrastructure;
using Godzilla.Events.Data;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DefaultEventQueue<TContext>> _logger;
        private readonly Dictionary<string, List<EventQueueSubscription<TContext>>> _subscriptions = new Dictionary<string, List<EventQueueSubscription<TContext>>>();

        public DefaultEventQueue(
            IEventQueueProvider<TContext> eventQueueProvider,
            ILogger<DefaultEventQueue<TContext>> logger)
        {
            _eventQueueProvider = eventQueueProvider;
            _logger = logger;
            _eventQueueProvider.OnEventReceived += ProcessEvent;
        }

        public async Task PublishEvent(EventData @event)
        {
            _logger.LogDebug($"Publishing event -> {@event.EventCategory} {@event.EventType} {@event.EventId}");
            await _eventQueueProvider.PublishEvent(@event);
        }

        private Task ProcessEvent(EventData @event)
        {
            _logger.LogDebug($"Processing event -> {@event.EventCategory} {@event.EventType} {@event.EventId}");

            if (!_subscriptions.ContainsKey(@event.EventCategory))
            {
                _logger.LogDebug($"No subscriptions for event -> {@event.EventCategory} {@event.EventType} {@event.EventId}");
                return Task.FromResult(false);
            }   

            var subscriptions = _subscriptions[@event.EventCategory];
            foreach (var subscription in subscriptions)
                InvokeCallback(subscription, @event);

            return Task.FromResult(false);
        }

        private void InvokeCallback(EventQueueSubscription<TContext> subscription, EventData @event)
        {
            _logger.LogDebug($"Invoke callback for event -> {@event.EventCategory} {@event.EventType} {@event.EventId}");
            Task.Run(() =>
            {
                _logger.LogDebug($"Invoking callback for event -> {@event.EventCategory} {@event.EventType} {@event.EventId}");
                subscription.Callback(@event);
            })
            .ConfigureAwait(false);
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
