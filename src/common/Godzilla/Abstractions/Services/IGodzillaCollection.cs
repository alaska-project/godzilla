using Godzilla.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    public interface IGodzillaCollection<TItem>
    {
        string CollectionId { get; }

        TItem GetItem(Guid id);
        IEnumerable<TItem> GetItems(IEnumerable<Guid> id);
        IQueryable<TItem> AsQueryable();

        void Add(TItem entity);
        void Add(IEnumerable<TItem> entities);

        void Update(TItem entity);
        void Update(IEnumerable<TItem> entities);

        void Delete(TItem entity);
        void Delete(IEnumerable<TItem> entities);
    }

    public interface IGodzillaCollection
    {
        string CollectionId { get; }

        void Add(object entity);
        void Add(IEnumerable<object> entities);
        void Update(object entity);
        void Update(IEnumerable<object> entities);
        void Delete(object entity);
        void Delete(IEnumerable<object> entities);
    }
}
