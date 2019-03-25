using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla
{
    public abstract class EntityRepository<TContext, TEntity>
        where TContext : EntityContext
    {
        #region Init

        public EntityRepository(TContext context)
        {
            Context = context;
        }

        protected TContext Context { get; }

        #endregion

        #region Entity

        protected virtual IQueryable<TEntity> AsQueryable()
        {
            return Context.Query.AsQueryable<TEntity>();
        }

        protected virtual async Task<TEntity> GetEntity(Guid id)
        {
            return await Context.Query.GetItem<TEntity>(id);
        }

        protected virtual async Task<TEntity> GetEntity(string path)
        {
            return await Context.Query.GetItem<TEntity>(path);
        }

        protected virtual async Task<TEntity> GetEntity(Expression<Func<TEntity, bool>> filter)
        {
            return await Context.Query.GetItem(filter);
        }

        protected virtual async Task<IEnumerable<TEntity>> GetEntities()
        {
            return await Context.Query.GetItems<TEntity>(x => true);
        }

        protected virtual async Task<IEnumerable<TEntity>> GetEntities(IEnumerable<Guid> id)
        {
            return await Context.Query.GetItems<TEntity>(id);
        }

        protected virtual async Task<IEnumerable<TEntity>> GetEntities(Expression<Func<TEntity, bool>> filter)
        {
            return await Context.Query.GetItems(filter);
        }

        #endregion
    }
}
