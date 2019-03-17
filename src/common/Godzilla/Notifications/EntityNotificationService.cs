using Godzilla.Abstractions;
using Godzilla.Abstractions.Infrastructure;
using Godzilla.Events.Data;
using Godzilla.Notifications.Collections;
using Godzilla.Notifications.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private readonly IDisposable _eventQueueSubscription;

        public EntityNotificationService(IEventQueue<TContext> eventQueue)
        {
            _eventQueue = eventQueue;
            _eventQueueSubscription = eventQueue.SubscribeEvent(EventNotificationsCategory, ProcessEvent);
        }

        public IEntitySubscription SubscribeEntityEvent(Guid entityId, Action callback)
        {
            var subscription = new EntityNotificationSubscription<TContext>(entityId, this, callback);
            _subscriptions.AddSubscription(subscription);
            return subscription;
        }

        internal void Unsubscribe(EntityNotificationSubscription<TContext> subscription)
        {
            _subscriptions.RemoveSubscription(subscription);
        }

        public async Task PublishEntityEvent<TEvent>(TEvent eventData)
        {
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
            foreach (var subscription in subscriptions)
                subscription.InvokeCallback();
        }

        private T DeserializeEventData<T>(EventData eventData)
        {
            return JsonConvert.DeserializeObject<T>(eventData.Data);
        }
    }
}
