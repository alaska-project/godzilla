using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Collections.Internal
{
    internal class GodzillaCollection<TItem> : IGodzillaCollection<TItem>
    {
        protected readonly IDatabaseCollection<TItem> _collection;

        public GodzillaCollection(IDatabaseCollection<TItem> collection)
        {
            _collection = collection;
        }

        public IQueryable<TItem> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public IQueryable<TDerived> AsQueryable<TDerived>() where TDerived : TItem
        {
            return _collection.AsQueryable<TDerived>();
        }

        public void Add(TItem entity)
        {
            _collection.Add(entity);
        }

        public void Add<TDerived>(TDerived entity) 
            where TDerived : TItem
        {
            _collection.Add(entity);
        }
        
        public void Delete(TItem entity)
        {
            _collection.Delete(entity);
        }

        public void Delete<TDerived>(TItem entity) 
            where TDerived : TItem
        {
            _collection.Delete(entity);
        }

        public void Update(TItem entity)
        {
            _collection.Update(entity);
        }

        public void Update<TDerived>(TDerived entity) where TDerived : TItem
        {
            _collection.Update(entity);
        }
    }
}
