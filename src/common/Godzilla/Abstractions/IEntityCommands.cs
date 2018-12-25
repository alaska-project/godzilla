using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions
{
    public interface IEntityCommands
    {
        Task<TEntity> Add<TEntity>(TEntity entity);
        Task<IEnumerable<TEntity>> Add<TEntity>(IEnumerable<TEntity> entity);
    }
}
