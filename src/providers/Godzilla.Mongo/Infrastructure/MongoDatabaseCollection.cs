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

        public IQueryable<TDerived> AsQueryable<TDerived>() where TDerived : TItem
        {
            return _collection
                .OfType<TDerived>()
                .AsQueryable();
        }

        public async Task Add(TItem entity)
        {
            await _collection
                .InsertOneAsync(entity);
        }

        public async Task Add<TDerived>(TDerived entity) where TDerived : TItem
        {
            await _collection
                .OfType<TDerived>()
                .InsertOneAsync(entity);
        }
        
        public void Delete(TItem entity)
        {
            throw new NotImplementedException();
        }

        public void Delete<TDerived>(TItem entity) where TDerived : TItem
        {
            throw new NotImplementedException();
        }

        public void Update(TItem entity)
        {
            throw new NotImplementedException();
        }

        public void Update<TDerived>(TDerived entity) where TDerived : TItem
        {
            throw new NotImplementedException();
        }
    }
}
