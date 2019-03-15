using Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Notifications
{
    internal class EntityNotificationService<TContext> : IEntityNotificationService<TContext>
        where TContext : EntityContext
    {
    }
}
