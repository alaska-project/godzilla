using Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Notifications
{
    internal class EntityNotificationSubscription<TContext> : IEntitySubscription
        where TContext : EntityContext
    {
        private readonly EntityNotificationService<TContext> _notificationService;
        private readonly Action _callback;

        public Guid EntityId { get; }

        public EntityNotificationSubscription(
            Guid entityId,
            EntityNotificationService<TContext> notificationService,
            Action callback)
        {
            EntityId = entityId;
            _notificationService = notificationService;
            _callback = callback;
        }

        public void InvokeCallback()
        {
            _callback.Invoke();
        }

        public void Dispose()
        {
            _notificationService.Unsubscribe(this);
        }
    }
}
