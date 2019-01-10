using Godzilla.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Services
{
    public interface IGodzillaCollection<TItem>
    {
        string CollectionId { get; }

        TItem GetItem(Guid id);
        IEnumerable<TItem> GetItems(IEnumerable<Guid> id);
        IEnumerable<TItem> GetItems(IEnumerable<Guid> id, Expression<Func<TItem, bool>> filter);
        IQueryable<TItem> AsQueryable();

        Task Add(TItem entity);
        Task Add(IEnumerable<TItem> entities);

        Task Update(TItem entity);
        Task Update(IEnumerable<TItem> entities);

        Task Delete(TItem entity);
        Task Delete(IEnumerable<TItem> entities);

        Task CreateIndex(IndexDefinition<TItem> index);
    }

    public interface IGodzillaCollection
    {
        string CollectionId { get; }

        Task Add(object entity);
        Task Add(IEnumerable<object> entities);
        Task Update(object entity);
        Task Update(IEnumerable<object> entities);
        Task Delete(object entity);
        Task Delete(IEnumerable<object> entities);
        Task Delete(IEnumerable<Guid> entities);
    }
}
