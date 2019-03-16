using Godzilla.Mongo.Abstractions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Mongo.EventQueue
{
    internal class MongoCollectionPollingWatcher : MongoCollectionWatcherBase<EventRecord>
    {
        private readonly TimeSpan _pollingInterval;
        private ObjectId? _lastProcessedEventId;
        private DateTime _pollingStartTime;

        public MongoCollectionPollingWatcher(IMongoCollection<EventRecord> collection, TimeSpan pollingInterval) : base(collection)
        {
            _pollingInterval = pollingInterval;
        }

        public override void Watch(Action<EventRecord> callback)
        {
            _pollingStartTime = DateTime.Now;

            while (true)
            {
                IQueryable<EventRecord> queryableEvents = Collection
                    .AsQueryable();

                if (_lastProcessedEventId.HasValue)
                    queryableEvents = queryableEvents
                        .Where(x => x.Id > _lastProcessedEventId);
                else
                    queryableEvents = queryableEvents
                        .Where(x => x.Data.EventTime >= _pollingStartTime);

                var events = queryableEvents
                    .OrderBy(x => x.Id)
                    .ToList();

                if (events.Any())
                {
                    foreach (var @event in events)
                        InvokeCallback(callback, @event);

                    _lastProcessedEventId = events.Last().Id;
                }

                Thread.Sleep(_pollingInterval);
            }
        }
    }
}
