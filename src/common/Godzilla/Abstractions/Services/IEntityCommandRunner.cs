using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Services
{
    public interface IEntityCommandRunner
    {
        Task<TEntity> Add<TEntity>(TEntity entity);
        Task<IEnumerable<TEntity>> Add<TEntity>(IEnumerable<TEntity> entity);
    }
}
