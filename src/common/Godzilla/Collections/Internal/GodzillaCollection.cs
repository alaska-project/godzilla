using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public virtual async Task Add(TItem entity)
        {
            await _collection.Add(entity);
        }

        public virtual async Task Add(IEnumerable<TItem> entities)
        {
            await _collection.Add(entities);
        }
                
        public virtual async Task Delete(TItem entity)
        {
            await _collection.Delete(entity);
        }

        public virtual async Task Delete(IEnumerable<TItem> entities)
        {
            await _collection.Delete(entities);
        }

        public async Task Delete(IEnumerable<Guid> id)
        {
            await _collection.Delete(id);
        }

        public virtual async Task Update(TItem entity)
        {
            await _collection.Update(entity);
        }

        public virtual async Task Update(IEnumerable<TItem> entities)
        {
            await _collection.Update(entities);
        }

        public virtual async Task CreateIndex(IndexDefinition<TItem> index)
        {
            await _collection.CreateIndex(index.Name, index.Fields, index.Options);
        }

        async Task IGodzillaCollection.Add(object entity)
        {
            await Add((TItem)entity);
        }

        async Task IGodzillaCollection.Add(IEnumerable<object> entities)
        {
            await Add(entities.Cast<TItem>());
        }

        async Task IGodzillaCollection.Update(object entity)
        {
            await Update((TItem)entity);
        }

        async Task IGodzillaCollection.Update(IEnumerable<object> entities)
        {
            await Update(entities.Cast<TItem>());
        }

        async Task IGodzillaCollection.Delete(object entity)
        {
            await Delete((TItem)entity);
        }

        async Task IGodzillaCollection.Delete(IEnumerable<object> entities)
        {
            await Delete(entities.Cast<TItem>());
        }
    }
}
