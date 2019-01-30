using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions
{
    public interface IEntityCommands
    {
        Task<TEntity> Add<TEntity>(TEntity entity);
        Task<IEnumerable<TEntity>> Add<TEntity>(IEnumerable<TEntity> entities);

        Task<TEntity> Add<TEntity>(TEntity entity, Guid parentId);
        Task<IEnumerable<TEntity>> Add<TEntity>(IEnumerable<TEntity> entities, Guid parentId);

        Task<TEntity> Add<TEntity>(TEntity entity, object parent);
        Task<IEnumerable<TEntity>> Add<TEntity>(IEnumerable<TEntity> entities, object parent);

        Task<TEntity> Update<TEntity>(TEntity entity);
        Task<IEnumerable<TEntity>> Update<TEntity>(IEnumerable<TEntity> entities);
        Task<TEntity> Update<TEntity, TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value);
        Task<IEnumerable<TEntity>> Update<TEntity, TField>(IEnumerable<TEntity> entities, Expression<Func<TEntity, TField>> field, TField value);
        Task<TEntity> Update<TEntity, TField>(Guid entityId, Expression<Func<TEntity, TField>> field, TField value);
        Task<IEnumerable<TEntity>> Update<TEntity, TField>(IEnumerable<Guid> idList, Expression<Func<TEntity, TField>> field, TField value);

        Task Delete<TEntity>(TEntity entity);
        Task Delete<TEntity>(IEnumerable<TEntity> entities);

        Task Move<TEntity>(TEntity entity, Guid newParentId);
        Task Move<TEntity>(TEntity entity, object newParent);
        Task Move(Guid entityId, Guid newParentId);

        Task Rename<TEntity>(TEntity entity, string newName);
        Task Rename(Guid entityId, string newName);
    }
}
