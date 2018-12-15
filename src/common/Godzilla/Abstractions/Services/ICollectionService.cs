using Godzilla.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    public interface ICollectionService<TContext>
        where TContext : EntityContext
    {
        IDatabaseCollection<TEntity> GetCollection<TEntity>();
    }
}
