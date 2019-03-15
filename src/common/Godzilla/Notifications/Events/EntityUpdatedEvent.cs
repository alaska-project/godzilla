using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Notifications.Events
{
    public class EntityUpdatedEvent
    {
        public Guid EntityId { get; set; }
    }
}
