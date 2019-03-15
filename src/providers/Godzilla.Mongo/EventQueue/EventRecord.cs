using Godzilla.Events.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.EventQueue
{
    internal class EventRecord
    {
        public Guid Id { get; set; }
        public EventData Data { get; set; }
    }
}
