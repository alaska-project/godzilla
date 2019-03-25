using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla
{
    public abstract class DocumentRepository<TContext, TEntity, TAggregate> :
        DocumentRepository<TContext, TEntity>
        where TContext : EntityContext
        where TAggregate : DocumentAggregate<TEntity>
    {
        #region Init

        public DocumentRepository(TContext context)
            : base(context)
        { }

        #endregion

        #region Aggregate

        protected virtual async Task<TAggregate> GetOrCreateAggregate(Expression<Func<TEntity, bool>> filter, Func<TEntity> entityInitialization)
        {
            var aggregate = await GetAggregate(filter);
            if (aggregate != null)
                return aggregate;

            var newEntity = entityInitialization();
            var newDocument = await Context.Documents.CreateDocument(newEntity);
            return await CreateAggregateInstance(newDocument);
        }

        protected virtual async Task<TAggregate> GetAggregate(Guid id)
        {
            var document = await GetDocument(id);
            return document == null ?
                null :
                await CreateAggregateInstance(document);
        }

        protected virtual async Task<TAggregate> GetAggregate(string path)
        {
            var document = await GetDocument(path);
            return document == null ?
                null :
                await CreateAggregateInstance(document);
        }

        protected virtual async Task<TAggregate> GetAggregate(Expression<Func<TEntity, bool>> filter)
        {
            var document = await GetDocument(filter);
            return document == null ?
                null :
                await CreateAggregateInstance(document);
        }

        protected virtual async Task<IEnumerable<TAggregate>> GetAggregates()
        {
            var documents = await GetDocuments();
            return await CreateAggregateInstances(documents);
        }

        protected virtual async Task<IEnumerable<TAggregate>> GetAggregates(IEnumerable<Guid> id)
        {
            var documents = await GetDocuments(id);
            return await CreateAggregateInstances(documents);
        }

        protected virtual async Task<IEnumerable<TAggregate>> GetAggregates(Expression<Func<TEntity, bool>> filter)
        {
            var documents = await GetDocuments(filter);
            return await CreateAggregateInstances(documents);
        }

        protected virtual Task<TAggregate> CreateAggregateInstance(Document<TEntity> document)
        {
            var aggregate = (TAggregate) Activator.CreateInstance(typeof(TAggregate), new object[] { document });
            return Task.FromResult(aggregate);
        }

        private async Task<IEnumerable<TAggregate>> CreateAggregateInstances(IEnumerable<Document<TEntity>> documents)
        {
            var aggregates = new List<TAggregate>();
            foreach (var document in documents)
                aggregates.Add(await CreateAggregateInstance(document));
            return aggregates;
        }

        #endregion
    }

    public abstract class DocumentRepository<TContext, TEntity>
        where TContext : EntityContext
    {
        #region Init

        public DocumentRepository(TContext context)
        {
            Context = context;
        }

        protected TContext Context { get; }

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
    }
}
