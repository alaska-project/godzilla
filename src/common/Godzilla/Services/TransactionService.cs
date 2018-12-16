using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Services
{
    internal class TransactionService<TContext> :
        ITransactionService<TContext>
        where TContext : EntityContext
    {
        private readonly ICollectionInitializer _initializer;
        private readonly ICollectionResolver<TContext> _resolver;
        private readonly IDatabaseTransactionManager<TContext> _transactionManager;

        public TransactionService(
            ICollectionInitializer initializer,
            ICollectionResolver<TContext> resolver,
            IDatabaseTransactionManager<TContext> transactionManager)
        {
            _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _transactionManager = transactionManager ?? throw new ArgumentNullException(nameof(transactionManager));
        }

        public void StartTransaction()
        {
            _transactionManager.StartTransaction();
        }

        public void AbortTransaction()
        {
            _transactionManager.AbortTransaction();
        }

        public void CommitTransaction()
        {
            _transactionManager.CommitTransaction();
        }
        
        public IGodzillaCollection<TItem> GetCollection<TItem>()
        {
            var collectionInfo = _resolver.GetCollectionInfo<TItem>();
            var collection = _transactionManager.GetCollection<TItem>(collectionInfo.CollectionId);
            return _initializer.CreateCollection(collection);
        }

        public TCollection GetCollection<TItem, TCollection>() where TCollection : IGodzillaCollection<TItem>
        {
            var collectionInfo = _resolver.GetCollectionInfo<TItem>();
            var collection = _transactionManager.GetCollection<TItem>(collectionInfo.CollectionId);
            return _initializer.CreateCollection<TItem, TCollection>(collection);
        }
    }
}
