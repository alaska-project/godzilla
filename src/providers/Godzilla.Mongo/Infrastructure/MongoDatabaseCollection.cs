using Godzilla.Abstractions.Infrastructure;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Mongo.Infrastructure
{
    internal class MongoDatabaseCollection<TItem> : IDatabaseCollection<TItem>
    {
        protected IMongoCollection<TItem> _collection;

        public MongoDatabaseCollection(IMongoCollection<TItem> collection)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

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
        
        public Task Delete(TItem entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(IEnumerable<TItem> entities)
        {
            throw new NotImplementedException();
        }

        public Task Update(TItem entity)
        {
            throw new NotImplementedException();
        }
        
        public Task Update(IEnumerable<TItem> entities)
        {
            throw new NotImplementedException();
        }
    }
}
