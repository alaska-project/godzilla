using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public string CollectionId => _collection.CollectionId;

        public virtual IQueryable<TItem> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual TItem GetItem(Guid id)
        {
            return _collection.GetItem(id);
        }

        public virtual IEnumerable<TItem> GetItems(IEnumerable<Guid> id)
        {
            return _collection.GetItems(id);
        }

        public IEnumerable<TItem> GetItems(IEnumerable<Guid> id, Expression<Func<TItem, bool>> filter)
        {
            return _collection.GetItems(id, filter);
        }

        public virtual void Add(TItem entity)
        {
            _collection.Add(entity).ConfigureAwait(false);
        }

        public virtual void Add(IEnumerable<TItem> entities)
        {
            _collection.Add(entities).ConfigureAwait(false);
        }
                
        public virtual void Delete(TItem entity)
        {
            _collection.Delete(entity);
        }

        public virtual void Delete(IEnumerable<TItem> entities)
        {
            _collection.Delete(entities);
        }

        public virtual void Update(TItem entity)
        {
            _collection.Update(entity);
        }

        public virtual void Update(IEnumerable<TItem> entities)
        {
            _collection.Update(entities);
        }

        void IGodzillaCollection.Add(object entity)
        {
            Add((TItem)entity);
        }

        void IGodzillaCollection.Add(IEnumerable<object> entities)
        {
            Add(entities.Cast<TItem>());
        }

        void IGodzillaCollection.Update(object entity)
        {
            Update((TItem)entity);
        }

        void IGodzillaCollection.Update(IEnumerable<object> entities)
        {
            Update(entities.Cast<TItem>());
        }

        void IGodzillaCollection.Delete(object entity)
        {
            Delete((TItem)entity);
        }

        void IGodzillaCollection.Delete(IEnumerable<object> entities)
        {
            Delete(entities.Cast<TItem>());
        }
    }
}
