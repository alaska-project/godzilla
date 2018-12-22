using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Services
{
    internal interface IEntityCommandRunner
    {
        Task<TEntity> Add<TEntity>(TEntity entity);
    }
}
