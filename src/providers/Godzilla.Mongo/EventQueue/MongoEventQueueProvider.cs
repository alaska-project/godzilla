using Godzilla.Abstractions.Infrastructure;
using Godzilla.Events.Data;
using Godzilla.Mongo.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Mongo.EventQueue
{
    internal class MongoEventQueueProvider<TContext> : IEventQueueProvider<TContext>
        where TContext : EntityContext
    {
        private readonly MongoDatabaseFactory<TContext> _databaseFactory;
        private readonly IMongoCollection<EventRecord> _eventsCollection;
        private EventReceivedHandler _eventReceivedHandler;
        private bool _isWatching = false;

        public MongoEventQueueProvider(MongoDatabaseFactory<TContext> databaseFactory)
        {
            _databaseFactory = databaseFactory;
            _eventsCollection = databaseFactory.GetMongoCollection<EventRecord>("_Events");
        }
        
        public async Task PublishEvent(EventData @event)
        {
            await _eventsCollection.InsertOneAsync(new EventRecord
            {
                Id = Guid.NewGuid(),
                Data = @event,
            });
        }

        public EventReceivedHandler OnEventReceived
        {
            get { return _eventReceivedHandler; }
            set
            {
                lock (this)
                {
                    WatchCollection(_eventsCollection);
                    _eventReceivedHandler = value;
                }
            }
        }

        private void WatchCollection(IMongoCollection<EventRecord> collection)
        {
            if (_isWatching)
                return;

            _isWatching = true;

            var options = new ChangeStreamOptions()
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
            };

            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<EventRecord>>().Match("{ operationType: { $in: [ 'insert' ] } }");

            var changesCursor = collection
                .Watch(pipeline, options)
                .ToEnumerable()
                .GetEnumerator();

            while (changesCursor.MoveNext())
            {
                var eventData = changesCursor.Current.FullDocument.Data;
                _eventReceivedHandler?.Invoke(eventData);
            }
        }
    }
}
