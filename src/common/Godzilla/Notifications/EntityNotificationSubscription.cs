using Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Notifications
{
    public enum EntityNotificationState { Active, Disposed }

    internal class EntityNotificationSubscription<TContext> : IEntitySubscription
        where TContext : EntityContext
    {
        private readonly EntityNotificationService<TContext> _notificationService;
        private readonly Action _callback;
        private EntityNotificationState _state = EntityNotificationState.Active;

        public Guid EntityId { get; }
        public Guid SubscriptionId { get; } = Guid.NewGuid();
        public EntityNotificationState State => _state;

        public EntityNotificationSubscription(
            Guid entityId,
            EntityNotificationService<TContext> notificationService,
            Action callback)
        {
            EntityId = entityId;
            _notificationService = notificationService;
            _callback = callback;
        }

        ~EntityNotificationSubscription()
        {
            if (_state == EntityNotificationState.Active)
                _notificationService.Destroy(this);
        }

        public void InvokeCallback()
        {
            _callback.Invoke();
        }

        public void Dispose()
        {
            _notificationService.Unsubscribe(this);
            _state = EntityNotificationState.Disposed;
        }
    }
}
