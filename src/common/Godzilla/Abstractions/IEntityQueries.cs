using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions
{
    public interface IEntityQueries
    {
        IQueryable<TEntity> AsQueryable<TEntity>();
        Task<TItem> GetItem<TItem>(Guid id);
        Task<TItem> GetItem<TItem>(string path);
        Task<TItem> GetItem<TItem>(Expression<Func<TItem, bool>> filter);
        Task<IEnumerable<TItem>> GetItems<TItem>(string path);
        Task<IEnumerable<TItem>> GetItems<TItem>(IEnumerable<Guid> id);
        Task<IEnumerable<TItem>> GetItems<TItem>(Expression<Func<TItem, bool>> filter);
        Task<TParent> GetParent<TParent>(object entity);
        Task<TChild> GetChild<TChild>(object entity);
        Task<TChild> GetChild<TChild>(object entity, Expression<Func<TChild, bool>> filter);
        Task<IEnumerable<TChild>> GetChildren<TChild>(object entity);
        Task<IEnumerable<TChild>> GetChildren<TChild>(object entity, Expression<Func<TChild, bool>> filter);
    }
}
