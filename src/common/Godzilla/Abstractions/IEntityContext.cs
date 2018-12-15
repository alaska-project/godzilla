using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Abstractions
{
    public interface IEntityContext
    {
        IQueryable<TEntity> AsQueryable<TEntity>();
    }
}
