using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Collections.Internal
{
    internal class GodzillaCollection<TItem> :
        IGodzillaCollection,
        IGodzillaCollection<TItem>
    {
        protected readonly IDatabaseCollection<TItem> _collection;

        public GodzillaCollection(IDatabaseCollection<TItem> collection)
        {
            _collection = collection;
        }

        public virtual IQueryable<TItem> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IQueryable<TDerived> AsQueryable<TDerived>() where TDerived : TItem
        {
            return _collection.AsQueryable<TDerived>();
        }

        public virtual void Add(TItem entity)
        {
            _collection.Add(entity);
        }

        public virtual void Add<TDerived>(TDerived entity) 
            where TDerived : TItem
        {
            _collection.Add(entity);
        }
        
        public virtual void Delete(TItem entity)
        {
            _collection.Delete(entity);
        }

        public virtual void Delete<TDerived>(TItem entity) 
            where TDerived : TItem
        {
            _collection.Delete(entity);
        }

        public virtual void Update(TItem entity)
        {
            _collection.Update(entity);
        }

        public virtual void Update<TDerived>(TDerived entity) where TDerived : TItem
        {
            _collection.Update(entity);
        }

        void IGodzillaCollection.Add(object entity)
        {
            Add((TItem)entity);
        }

        void IGodzillaCollection.Update(object entity)
        {
            Update((TItem)entity);
        }

        void IGodzillaCollection.Delete(object entity)
        {
            Delete((TItem)entity);
        }
    }
}
