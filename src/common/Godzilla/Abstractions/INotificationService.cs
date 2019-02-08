using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Abstractions
{
    public interface INotificationService<TContext> : INotificationService
        where TContext : EntityContext
    { }

    public interface INotificationService
    {
        Task PublishEvent(object @event, CancellationToken cancellationToken = default(CancellationToken));
    }
}
