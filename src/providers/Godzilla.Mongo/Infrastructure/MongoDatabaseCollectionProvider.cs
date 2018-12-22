using Godzilla.Abstractions.Infrastructure;
using Godzilla.Mongo.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.Infrastructure
{
    internal class MongoDatabaseCollectionProvider<TContext> : IDatabaseCollectionProvider<TContext>
        where TContext : EntityContext
    {
        private readonly MongoDatabaseFactory<TContext> _factory;

        public MongoDatabaseCollectionProvider(MongoDatabaseFactory<TContext> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IDatabaseCollection<TItem> GetCollection<TItem, TBaseItem>(string collectionId) 
            where TItem : TBaseItem
        {
            var collection = _factory.GetMongoCollection<TItem, TBaseItem>(collectionId);
            return new MongoDatabaseCollection<TItem>(collection);
        }        
    }
}
