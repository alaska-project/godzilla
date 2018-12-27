using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Mongo.Infrastructure
{
    internal class MongoDatabaseCollection<TContext, TItem> : IDatabaseCollection<TItem>
        where TContext : EntityContext
    {
        private readonly IEntityPropertyResolver<TContext> _propertyResolver;
        protected readonly IMongoCollection<TItem> _collection;

        public MongoDatabaseCollection(
            IEntityPropertyResolver<TContext> propertyResolver,
            IMongoCollection<TItem> collection, 
            string collectionId)
        {
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            CollectionId = collectionId;
        }

        public string CollectionId { get; }

        public IQueryable<TItem> AsQueryable()
        {
            return _collection
                .AsQueryable();
        }
        
        public async Task Add(TItem entity)
        {
            await _collection
                .InsertOneAsync(entity);
        }

        public async Task Add(IEnumerable<TItem> entities)
        {
            await _collection
                .InsertManyAsync(entities);
        }
        
        public async Task Update(TItem entity)
        {
            var id = _propertyResolver.GetEntityId(entity);
            await _collection.ReplaceOneAsync(Builders<TItem>.Filter.Eq("_id", id), entity);
        }
        
        public async Task Update(IEnumerable<TItem> entities)
        {
            foreach (var entity in entities)
                await Update(entity);
        }

        public async Task Delete(TItem entity)
        {
            var id = _propertyResolver.GetEntityId(entity);
            await _collection.DeleteOneAsync(Builders<TItem>.Filter.Eq("_id", id));
        }

        public async Task Delete(IEnumerable<TItem> entities)
        {
            var entitiesId = entities
                .Select(x => _propertyResolver.GetEntityId(x))
                .ToList();

            var filter = Builders<TItem>.Filter.In("_id", entitiesId);
            await _collection.DeleteManyAsync(filter);
        }

        public async Task Delete(Expression<Func<TItem, bool>> filter)
        {
            await _collection.DeleteManyAsync(filter);
        }
    }
}
