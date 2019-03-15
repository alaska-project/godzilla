using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Notifications.Collections
{
    internal class ConcurrentEntitySubscriptionsDictionary<TContext>
        where TContext : EntityContext
    {
        private Dictionary<Guid, List<EntityNotificationSubscription<TContext>>> _subscriptions = new Dictionary<Guid, List<EntityNotificationSubscription<TContext>>>();

        public void AddSubscription(EntityNotificationSubscription<TContext> subscription)
        {
            lock (this)
            {
                if (!_subscriptions.ContainsKey(subscription.EntityId))
                    _subscriptions.Add(subscription.EntityId, new List<EntityNotificationSubscription<TContext>>());
                _subscriptions[subscription.EntityId].Add(subscription);
            }
        }

        public void RemoveSubscription(EntityNotificationSubscription<TContext> subscription)
        {
            lock (this)
            {
                if (!_subscriptions.ContainsKey(subscription.EntityId))
                    throw new InvalidOperationException($"No subscriptions found for entity {subscription.EntityId}");
                var subscriptions = _subscriptions[subscription.EntityId];
                subscriptions.Remove(subscription);
                if (!subscriptions.Any())
                    _subscriptions.Remove(subscription.EntityId);
            }
        }

        public IEnumerable<EntityNotificationSubscription<TContext>> GetSubscriptions(Guid entityId)
        {
            lock (this)
            {
                return _subscriptions.ContainsKey(entityId) ?
                    _subscriptions[entityId] :
                    new List<EntityNotificationSubscription<TContext>>();
            }
        }
    }
}
