using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using Godzilla.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
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

        public virtual IGodzillaCollection GetCollection(Type itemType)
        {
            var getCollectionMethod = ReflectionUtil.GetGenericMethod(this.GetType(), "GetCollection", BindingFlags.Instance | BindingFlags.Public);
            var specificGetCollectionMethod = getCollectionMethod.MakeGenericMethod(itemType);
            return (IGodzillaCollection)specificGetCollectionMethod.Invoke(this, new object[0]);
        }

        public virtual IGodzillaCollection<TItem> GetCollection<TItem>()
        {
            var collection = _resolver.GetCollection<TItem>(_transactionManager);
            return _initializer.CreateCollection(collection);
        }

        public virtual TCollection GetCollection<TItem, TCollection>() where TCollection : IGodzillaCollection<TItem>
        {
            var collection = _resolver.GetCollection<TItem>(_transactionManager);
            return _initializer.CreateCollection<TItem, TCollection>(collection);
        }
    }
}
