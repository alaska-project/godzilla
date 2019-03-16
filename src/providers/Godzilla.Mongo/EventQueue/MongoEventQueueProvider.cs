using Godzilla.Abstractions.Infrastructure;
using Godzilla.Events.Data;
using Godzilla.Mongo.Abstractions;
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
        private MongoCollectionWatcherBase<EventRecord> _collectionWatcher;

        public MongoEventQueueProvider(MongoDatabaseFactory<TContext> databaseFactory)
        {
            _databaseFactory = databaseFactory;
            _eventsCollection = databaseFactory.GetMongoCollection<EventRecord>("_Events");
        }

        public async Task PublishEvent(EventData @event)
        {
            await _eventsCollection.InsertOneAsync(new EventRecord
            {
                Id = ObjectId.GenerateNewId(),
                Data = @event,
            });
        }

        public EventReceivedHandler OnEventReceived
        {
            get { return _eventReceivedHandler; }
            set
            {
                WatchCollection(_eventsCollection);
                _eventReceivedHandler = value;
            }
        }

        private void WatchCollection(IMongoCollection<EventRecord> collection)
        {
            lock (this)
            {
                if (_isWatching)
                    return;

                _collectionWatcher = GetCollectionWatcher(collection);
                _isWatching = true;
            }

            Task.Run(() =>
            {
                _collectionWatcher.Watch(x => _eventReceivedHandler?.Invoke(x.Data));
            });
        }

        private MongoCollectionWatcherBase<EventRecord> GetCollectionWatcher(IMongoCollection<EventRecord> collection)
        {
            if (IsReplicaSetEnabled(collection))
                return new MongoCollectionChangesStreamWatcher(collection);

            return new MongoCollectionPollingWatcher(collection, TimeSpan.FromMilliseconds(200));
        }

        private bool IsReplicaSetEnabled(IMongoCollection<EventRecord> collection)
        {
            //TODO: return !string.IsNullOrEmpty(collection.Database.Client.Settings.ReplicaSetName);
            return false;
        }
    }
}
