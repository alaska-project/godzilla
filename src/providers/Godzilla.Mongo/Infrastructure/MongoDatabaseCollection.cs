﻿using Godzilla.Abstractions.Infrastructure;
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
        private const string IdField = "_id";
        
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

        public async Task<TItem> GetItem(Guid id)
        {
            return await _collection
                .Find(GetEntityIdFilter(id))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TItem>> GetItems(IEnumerable<Guid> id)
        {
            return await _collection
                .Find(GetEntityIdFilter(id))
                .ToListAsync();
        }

        public async Task<IEnumerable<TItem>> GetItems(IEnumerable<Guid> id, Expression<Func<TItem, bool>> filter)
        {
            var idFilter = GetEntityIdFilter(id);
            var expressionFilter = GetExpressionFilter(filter);
            var collectionFilter = Builders<TItem>.Filter.And(idFilter, expressionFilter);

            return await _collection
                .Find(collectionFilter)
                .ToListAsync();
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
            await _collection.ReplaceOneAsync(GetEntityIdFilter(id), entity);
        }

        public async Task Update<TField>(TItem entity, Expression<Func<TItem, TField>> field, TField value)
        {
            var id = _propertyResolver.GetEntityId(entity);
            await Update(id, field, value);
        }

        public async Task Update<TField>(Guid id, Expression<Func<TItem, TField>> field, TField value)
        {
            await _collection.UpdateOneAsync(GetEntityIdFilter(id), Builders<TItem>.Update.Set(field, value));
        }

        public async Task Update(IEnumerable<TItem> entities)
        {
            foreach (var entity in entities)
                await Update(entity);
        }

        public async Task Update<TField>(IEnumerable<TItem> entities, Expression<Func<TItem, TField>> field, TField value)
        {
            var ids = entities.Select(x => _propertyResolver.GetEntityId(x));
            await Update(ids, field, value);
        }

        public async Task Update<TField>(IEnumerable<Guid> idList, Expression<Func<TItem, TField>> field, TField value)
        {
            await _collection.UpdateManyAsync(GetEntityIdFilter(idList), Builders<TItem>.Update.Set(field, value));
        }

        public async Task Delete(TItem entity)
        {
            var id = _propertyResolver.GetEntityId(entity);
            await _collection.DeleteOneAsync(GetEntityIdFilter(id));
        }

        public async Task Delete(IEnumerable<TItem> entities)
        {
            var entitiesId = entities
                .Select(x => _propertyResolver.GetEntityId(x))
                .ToList();

            var filter = GetEntityIdFilter(entitiesId);
            await _collection.DeleteManyAsync(filter);
        }

        public async Task Delete(Expression<Func<TItem, bool>> filter)
        {
            await _collection.DeleteManyAsync(filter);
        }

        public async Task Delete(IEnumerable<Guid> id)
        {
            await _collection
                .DeleteManyAsync(GetEntityIdFilter(id));
        }

        public async Task CreateIndex(string name, IEnumerable<IIndexField<TItem>> fields, IIndexOptions options)
        {
            var indexDefinition = MongoIndexHelper.CreateIndexDefinition(fields, options);
            var indexOptions = MongoIndexHelper.CreateIndexOptions(name, options);
            await _collection.Indexes.CreateOneAsync(new CreateIndexModel<TItem>(indexDefinition, indexOptions));
        }

        private FilterDefinition<TItem> GetEntityIdFilter(Guid entityId)
        {
            return Builders<TItem>.Filter.Eq(IdField, entityId);
        }

        private FilterDefinition<TItem> GetEntityIdFilter(IEnumerable<Guid> entitiesId)
        {
            return Builders<TItem>.Filter.In(IdField, entitiesId);
        }

        private FilterDefinition<TItem> GetExpressionFilter(Expression<Func<TItem, bool>> filter)
        {
            return Builders<TItem>.Filter.Where(filter);
        }
    }
}
