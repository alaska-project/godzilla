using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Notifications.Events
{
    public class EntityCreatedEvent
    {
        public Guid EntityId { get; set; }
        public Guid ParentId { get; set; }
    }
}
