using Godzilla.Mongo.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.Services
{
    internal class MongoDatabaseFactory<TContext>
        where TContext : EntityContext
    {
        private readonly MongoClientFactory<TContext> _mongoClientFactory;

        public MongoDatabaseFactory(MongoClientFactory<TContext> mongoClientFactory)
        {
            _mongoClientFactory = mongoClientFactory ?? throw new ArgumentNullException(nameof(mongoClientFactory));
        }

        public MongoClient GetMongoClient()
        {
            return _mongoClientFactory.GetMongoClient();
        }

        public IMongoDatabase GetDatabase()
        {
            var client = GetMongoClient();
            return client.GetDatabase(DatabaseName);
        }

        public IMongoCollection<TItem> GetMongoCollection<TItem>(string collectionId)
        {
            return GetMongoCollection<TItem, TItem>(collectionId);
        }

        public IMongoCollection<BsonDocument> GetMongoCollection(string collectionId)
        {
            var database = GetDatabase();
            return GetMongoCollection(collectionId, database);
        }

        public IMongoCollection<BsonDocument> GetMongoCollection(string collectionId, IMongoDatabase database)
        {
            return database.GetCollection<BsonDocument>(collectionId);
        }

        public IMongoCollection<TItem> GetMongoCollection<TItem, TBaseItem>(string collectionId)
            where TItem : TBaseItem
        {
            var database = GetDatabase();
            return GetMongoCollection<TItem, TBaseItem>(collectionId, database);
        }

        public IMongoCollection<TItem> GetMongoCollection<TItem, TBaseItem>(string collectionId, IMongoDatabase database)
            where TItem : TBaseItem
        {
            if (typeof(TItem) == typeof(TBaseItem))
                return database.GetCollection<TItem>(collectionId);

            return database.GetCollection<TBaseItem>(collectionId)
                .OfType<TItem>();
        }

        public string DatabaseName => _mongoClientFactory.DatabaseName;
    }
}
