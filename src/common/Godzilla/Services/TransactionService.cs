using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Services
{
    internal class TransactionService<TContext> :
        ITransactionService<TContext>
        where TContext : EntityContext
    {
        private readonly ICollectionResolver<TContext> _resolver;
        private readonly IDatabaseTransactionManager<TContext> _transactionManager;

        public TransactionService(
            ICollectionResolver<TContext> resolver,
            IDatabaseTransactionManager<TContext> transactionManager)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _transactionManager = transactionManager ?? throw new ArgumentNullException(nameof(transactionManager));
        }

        public void AbortTransaction()
        {
            _transactionManager.AbortTransaction();
        }

        public void CommitTransaction()
        {
            _transactionManager.CommitTransaction();
        }
        
        public IDatabaseCollection<TEntity> GetCollection<TEntity>()
        {
            var collectionInfo = _resolver.GetCollectionInfo<TEntity>();
            return _transactionManager.GetCollection<TEntity>(collectionInfo.CollectionId);
        }
        public void StartTransaction()
        {
            _transactionManager.StartTransaction();
        }
    }
}
