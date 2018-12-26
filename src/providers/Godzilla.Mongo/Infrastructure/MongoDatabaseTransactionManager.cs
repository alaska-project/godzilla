using Godzilla.Abstractions.Infrastructure;
using Godzilla.Mongo.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.Infrastructure
{
    internal class MongoDatabaseTransactionManager<TContext> : IDatabaseTransactionManager<TContext>
        where TContext : EntityContext
    {
        private readonly MongoDatabaseFactory<TContext> _factory;
        private IClientSessionHandle _session;

        public MongoDatabaseTransactionManager(MongoDatabaseFactory<TContext> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public void StartTransaction()
        {
            if (_session != null)
            {
                throw new InvalidOperationException("Transaction already started");
            }

            _session = _factory.GetMongoClient().StartSession();
            _session.StartTransaction();
        }

        public void AbortTransaction()
        {
            if (_session == null)
            {
                throw new InvalidOperationException("Transaction already closed");
            }

            _session.AbortTransaction();
        }

        public void CommitTransaction()
        {
            if (_session == null)
            {
                throw new InvalidOperationException("Transaction already closed");
            }
            _session.CommitTransaction();
        }

        public IDatabaseCollection<TItem> GetCollection<TItem, TBaseItem>(string collectionId) where TItem : TBaseItem
        {
            if (_session == null)
            {
                throw new InvalidOperationException("No active transactions");
            }

            var database = _session.Client.GetDatabase(_factory.DatabaseName);
            var collection = _factory.GetMongoCollection<TItem, TBaseItem>(collectionId, database);

            return new MongoDatabaseCollection<TItem>(collection, collectionId);
        }
    }
}
