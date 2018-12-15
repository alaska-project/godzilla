using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Abstractions.Infrastructure
{
    public interface IDatabaseCollection<TEntity>
    {
        IQueryable<TEntity> AsQueryable();
        IQueryable<TDerived> AdQueryable<TDerived>() where TDerived : TEntity;

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
