using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
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
        private readonly IEntityPropertyResolver<TContext> _propertyResolver;
        private readonly MongoDatabaseFactory<TContext> _factory;

        public MongoDatabaseCollectionProvider(
            IEntityPropertyResolver<TContext> propertyResolver,
            MongoDatabaseFactory<TContext> factory)
        {
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IDatabaseCollection<TItem> GetCollection<TItem, TBaseItem>(string collectionId) 
            where TItem : TBaseItem
        {
            var collection = _factory.GetMongoCollection<TItem, TBaseItem>(collectionId);
            return new MongoDatabaseCollection<TContext, TItem>(_propertyResolver, collection, collectionId);
        }        
    }
}
