using Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Queries
{
    internal class EntityQueries<TContext> : IEntityQueries
        where TContext : EntityContext
    {
        public IQueryable<TEntity> AsQueryable<TEntity>()
        {
            throw new NotImplementedException();
        }
    }
}
