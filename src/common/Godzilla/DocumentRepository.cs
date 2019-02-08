using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla
{
    public abstract class DocumentRepository<TContext, TEntity, TAggregate>
        where TContext : EntityContext
        where TAggregate : DocumentAggregate<TEntity>
    {
        #region Init

        public DocumentRepository(TContext context)
        {
            Context = context;
        }

        protected TContext Context { get; }

        #endregion

        #region Aggregate

        protected virtual async Task<TAggregate> GetAggregate(Guid id)
        {
            var document = await GetDocument(id);
            return document == null ?
                null :
                CreateAggregateInstance(document);
        }

        protected virtual async Task<TAggregate> GetAggregate(string path)
        {
            var document = await GetDocument(path);
            return document == null ?
                null :
                CreateAggregateInstance(document);
        }

        protected virtual async Task<TAggregate> GetAggregate(Expression<Func<TEntity, bool>> filter)
        {
            var document = await GetDocument(filter);
            return document == null ?
                null :
                CreateAggregateInstance(document);
        }

        protected virtual async Task<IEnumerable<TAggregate>> GetAggregates()
        {
            var documents = await GetDocuments();
            return documents
                .Select(x => CreateAggregateInstance(x))
                .ToList();
        }

        protected virtual async Task<IEnumerable<TAggregate>> GetAggregates(IEnumerable<Guid> id)
        {
            var documents = await GetDocuments(id);
            return documents
                .Select(x => CreateAggregateInstance(x))
                .ToList();
        }

        protected virtual async Task<IEnumerable<TAggregate>> GetAggregates(Expression<Func<TEntity, bool>> filter)
        {
            var documents = await GetDocuments(filter);
            return documents
                .Select(x => CreateAggregateInstance(x))
                .ToList();
        }

        protected virtual TAggregate CreateAggregateInstance(Document<TEntity> document)
        {
            return (TAggregate) Activator.CreateInstance(typeof(TAggregate), new object[] { document });
        }

        #endregion

        #region Document

        protected virtual async Task<Document<TEntity>> GetDocument(Guid id)
        {
            return await Context.Documents.GetDocument<TEntity>(id);
        }

        protected virtual async Task<Document<TEntity>> GetDocument(string path)
        {
            return await Context.Documents.GetDocument<TEntity>(path);
        }

        protected virtual async Task<Document<TEntity>> GetDocument(Expression<Func<TEntity, bool>> filter)
        {
            return await Context.Documents.GetDocument<TEntity>(filter);
        }

        protected virtual async Task<IEnumerable<Document<TEntity>>> GetDocuments()
        {
            return await Context.Documents.GetDocuments<TEntity>(x => true);
        }

        protected virtual async Task<IEnumerable<Document<TEntity>>> GetDocuments(IEnumerable<Guid> id)
        {
            return await Context.Documents.GetDocuments<TEntity>(id);
        }

        protected virtual async Task<IEnumerable<Document<TEntity>>> GetDocuments(Expression<Func<TEntity, bool>> filter)
        {
            return await Context.Documents.GetDocuments(filter);
        }

        #endregion

        #region Entity

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
