using Godzilla.Events.Data;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.EventQueue
{
    internal class EventRecord
    {
        public ObjectId Id { get; set; }
        public EventData Data { get; set; }
    }
}
