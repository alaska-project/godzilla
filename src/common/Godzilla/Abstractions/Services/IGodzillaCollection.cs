using Godzilla.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    public interface IGodzillaCollection<TItem>
    {
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
        void Add(object entity);
        void Add(IEnumerable<object> entities);
        void Update(object entity);
        void Update(IEnumerable<object> entities);
        void Delete(object entity);
        void Delete(IEnumerable<object> entities);
    }
}
