using Godzilla.Mongo.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.Services
{
    internal class MongoDatabaseFactory<TContext>
        where TContext : EntityContext
    {
        private readonly MongoEntityContextOptions<TContext> _options;

        public MongoDatabaseFactory(MongoEntityContextOptions<TContext> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public MongoClient GetMongoClient()
        {
            return new MongoClient(new MongoUrl(_options.ConnectionString));
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

        public string DatabaseName => _options.DatabaseName;
    }
}
