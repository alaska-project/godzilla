using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Notifications.Events
{
    public class EntityMovedEvent
    {
        public Guid EntityId { get; set; }
        public Guid OldParentId { get; set; }
        public Guid NewParentId { get; set; }
    }
}
