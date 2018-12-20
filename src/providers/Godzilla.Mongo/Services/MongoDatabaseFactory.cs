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

        public string DatabaseName => _options.DatabaseName;
    }
}
