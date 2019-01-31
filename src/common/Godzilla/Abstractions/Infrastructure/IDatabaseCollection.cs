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
        Task<TItem> GetItem(Guid id);
        Task<IEnumerable<TItem>> GetItems(IEnumerable<Guid> id);
        Task<IEnumerable<TItem>> GetItems(IEnumerable<Guid> id, Expression<Func<TItem, bool>> filter);

        Task Add(TItem entity);
        Task Add(IEnumerable<TItem> entities);

        Task Update(TItem entity);
        Task Update(IEnumerable<TItem> entities);
        Task Update<TField>(TItem entity, Expression<Func<TItem, TField>> field, TField value);
        Task Update<TField>(IEnumerable<TItem> entities, Expression<Func<TItem, TField>> field, TField value);
        Task Update<TField>(Guid id, Expression<Func<TItem, TField>> field, TField value);
        Task Update<TField>(IEnumerable<Guid> idList, Expression<Func<TItem, TField>> field, TField value);

        Task Delete(TItem entity);
        Task Delete(IEnumerable<TItem> entities);
        Task Delete(Expression<Func<TItem, bool>> filter);
        Task Delete(IEnumerable<Guid> id);

        Task CreateIndex(string name, IEnumerable<IIndexField<TItem>> fields, IIndexOptions options);
    }
}
