using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Godzilla.Abstractions
{
    public interface IEntityQueries
    {
        IQueryable<TEntity> AsQueryable<TEntity>();
        TItem GetItem<TItem>(Guid id);
        TItem GetItem<TItem>(string path);
        TItem GetItem<TItem>(Expression<Func<TItem, bool>> filter);
        IEnumerable<TItem> GetItems<TItem>(string path);
        IEnumerable<TItem> GetItems<TItem>(IEnumerable<Guid> id);
        IEnumerable<TItem> GetItems<TItem>(Expression<Func<TItem, bool>> filter);
        TParent GetParent<TParent>(object entity);
        TChild GetChild<TChild>(object entity);
        TChild GetChild<TChild>(object entity, Expression<Func<TChild, bool>> filter);
        IEnumerable<TChild> GetChildren<TChild>(object entity);
        IEnumerable<TChild> GetChildren<TChild>(object entity, Expression<Func<TChild, bool>> filter);
    }
}
