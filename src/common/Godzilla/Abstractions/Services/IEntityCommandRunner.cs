using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Services
{
    internal interface IEntityCommandRunner
    {
        Task Add<TEntity>(TEntity entity);
    }
}
