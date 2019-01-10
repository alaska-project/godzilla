using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Services
{
    public interface IEntityConfigurator
    {
        Task DefineIndex<TEntity>(IndexDefinition<TEntity> index);
    }
}
