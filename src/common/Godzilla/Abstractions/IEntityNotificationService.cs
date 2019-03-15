using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions
{
    public interface IEntityNotificationService<TContext> : IEntityNotificationService
        where TContext : EntityContext
    { }

    public interface IEntityNotificationService
    {
        IDisposable SubscribeEntityEvent(Guid entityId, Action callback);
        Task PublishEntityEvent<TEvent>(TEvent eventData);
    }
}
