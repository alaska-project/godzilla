using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Infrastructure
{
    public interface IDatabaseTransactionManager<TContext>
        where TContext : EntityContext
    {
        void StartTransaction();
        void CommitTransaction();
        void AbortTransaction();
        IDatabaseCollection<TItem> GetCollection<TItem>(string collectionId);
    }
}
