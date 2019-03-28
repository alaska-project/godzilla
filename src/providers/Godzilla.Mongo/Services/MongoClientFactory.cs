using Godzilla.Mongo.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.Services
{
    internal class MongoClientFactory<TContext>
        where TContext : EntityContext
    {
        private readonly MongoEntityContextOptions<TContext> _options;
        private readonly MongoClient _mongoClient;

        public MongoClientFactory(MongoEntityContextOptions<TContext> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _mongoClient = new MongoClient(new MongoUrl(_options.ConnectionString));
        }

        public MongoClient GetMongoClient() => _mongoClient;

        public string DatabaseName => _options.DatabaseName;
    }
}
