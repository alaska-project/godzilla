using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Infrastructure
{
    public interface IDatabaseCollection<TItem>
    {
        string CollectionId { get; }
        IQueryable<TItem> AsQueryable();
        TItem GetItem(Guid id);
        IEnumerable<TItem> GetItems(IEnumerable<Guid> id);
        IEnumerable<TItem> GetItems(IEnumerable<Guid> id, Expression<Func<TItem, bool>> filter);

        Task Add(TItem entity);
        Task Add(IEnumerable<TItem> entities);

        Task Update(TItem entity);
        Task Update(IEnumerable<TItem> entities);

        Task Delete(TItem entity);
        Task Delete(IEnumerable<TItem> entities);
        Task Delete(Expression<Func<TItem, bool>> filter);
        Task Delete(IEnumerable<Guid> id);

        Task CreateIndex(string name, IEnumerable<IIndexField<TItem>> fields, IIndexOptions options);
    }
}
