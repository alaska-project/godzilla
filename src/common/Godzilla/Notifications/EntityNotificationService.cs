using Godzilla.Abstractions;
using Godzilla.Abstractions.Infrastructure;
using Godzilla.Events.Data;
using Godzilla.Notifications.Collections;
using Godzilla.Notifications.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Notifications
{
    internal class EntityNotificationService<TContext> : IEntityNotificationService<TContext>, IDisposable
        where TContext : EntityContext
    {
        private const string EventNotificationsCategory = "entity_notifications";
        private readonly ConcurrentEntitySubscriptionsDictionary<TContext> _subscriptions = new ConcurrentEntitySubscriptionsDictionary<TContext>();
        private readonly IEventQueue<TContext> _eventQueue;
        private readonly ILogger<EntityNotificationService<TContext>> _logger;
        private readonly IDisposable _eventQueueSubscription;

        public EntityNotificationService(
            IEventQueue<TContext> eventQueue,
            ILogger<EntityNotificationService<TContext>> logger)
        {
            _eventQueue = eventQueue;
            _logger = logger;
            _eventQueueSubscription = eventQueue.SubscribeEvent(EventNotificationsCategory, ProcessEvent);
        }

        public IEntitySubscription SubscribeEntityEvent(Guid entityId, Action callback)
        {
            var subscription = new EntityNotificationSubscription<TContext>(entityId, this, callback);
            _subscriptions.AddSubscription(subscription);

            _logger.LogDebug($"Subscribing entity event -> {entityId} -> subscription {subscription.SubscriptionId}");
            return subscription;
        }

        internal void Unsubscribe(EntityNotificationSubscription<TContext> subscription)
        {
            _logger.LogDebug($"Canceling subscription for entity -> {subscription.EntityId} -> Subscriptoon {subscription.SubscriptionId}");
            _subscriptions.RemoveSubscription(subscription);
        }

        internal void Destroy(EntityNotificationSubscription<TContext> subscription)
        {
            _logger.LogWarning($"Destroying subscription for entity -> {subscription.EntityId} -> Subscriptoon {subscription.SubscriptionId}");
            _subscriptions.RemoveSubscription(subscription);
        }

        public async Task PublishEntityEvent<TEvent>(TEvent eventData)
        {
            _logger.LogDebug($"Publishing entity event -> {eventData.GetType().Name} -> {JsonConvert.SerializeObject(eventData)}");
            await _eventQueue.PublishEvent(new EventData
            {
                EventId = Guid.NewGuid(),
                EventTime = DateTime.Now,
                EventCategory = EventNotificationsCategory,
                EventType = eventData.GetType().Name,
                Data = JsonConvert.SerializeObject(eventData),
            });
        }

        private void ProcessEvent(EventData eventData)
        {
            _logger.LogDebug($"Processing entity event -> {eventData.GetType().Name} -> {JsonConvert.SerializeObject(eventData)}");
            if (eventData.EventType == typeof(EntityUpdatedEvent).Name)
            {
                var data = DeserializeEventData<EntityUpdatedEvent>(eventData);
                InvokeCallbacks(data.EntityId);
                return;
            }

            if (eventData.EventType == typeof(EntityDeletedEvent).Name)
            {
                var data = DeserializeEventData<EntityDeletedEvent>(eventData);
                InvokeCallbacks(data.EntityId);
                return;
            }
        }

        public void Dispose()
        {
            _eventQueueSubscription.Dispose();
        }

        private void InvokeCallbacks(Guid entityId)
        {
            var subscriptions = _subscriptions.GetSubscriptions(entityId);
            if (!subscriptions.Any())
            {
                _logger.LogDebug($"No subscriptions for entity -> {entityId}");
                return;
            }

            foreach (var subscription in subscriptions)
            {
                _logger.LogDebug($"Invoking entity callback -> {entityId} -> subscription {subscription.SubscriptionId}");
                subscription.InvokeCallback();
            }
        }

        private T DeserializeEventData<T>(EventData eventData)
        {
            return JsonConvert.DeserializeObject<T>(eventData.Data);
        }
    }
}
