using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Events.Data
{
    public class EventData
    {
        DateTime EventTime { get; set; }
        public string EventCategory { get; set; }
        public string EventId { get; set; }
        public object Data { get; set; }
    }
}
