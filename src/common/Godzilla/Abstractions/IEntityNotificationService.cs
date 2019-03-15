using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions
{
    public interface IEntityNotificationService<TContext> : IEntityNotificationService
        where TContext : EntityContext
    { }

    public interface IEntityNotificationService
    {
    }
}
