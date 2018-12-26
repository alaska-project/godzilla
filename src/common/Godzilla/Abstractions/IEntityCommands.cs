using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions
{
    public interface IEntityCommands
    {
        Task<TEntity> Add<TEntity>(TEntity entity);
        Task<IEnumerable<TEntity>> Add<TEntity>(IEnumerable<TEntity> entities);

        Task<TEntity> Update<TEntity>(TEntity entity);
        Task<IEnumerable<TEntity>> Update<TEntity>(IEnumerable<TEntity> entities);

        Task Delete<TEntity>(TEntity entity);
        Task Delete<TEntity>(IEnumerable<TEntity> entities);
    }
}
