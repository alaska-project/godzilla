using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Events.Data
{
    public class EventData
    {
        public Guid EventId { get; set; }
        public DateTime EventTime { get; set; }
        public string EventCategory { get; set; }
        public string EventType { get; set; }
        public string Data { get; set; }
    }
}
