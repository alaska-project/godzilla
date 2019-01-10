using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    public interface IEntityConfigurator<TContext>
        where TContext : EntityContext
    {
        void DefineIndex<TEntity>(string name, IEnumerable<IndexField<TEntity>> fields, IndexOptions options);
    }
}
