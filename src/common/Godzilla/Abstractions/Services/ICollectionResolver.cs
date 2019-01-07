using Godzilla.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    public interface ICollectionResolver<TContext>
        where TContext : EntityContext
    {
        IDatabaseCollection<TItem> GetCollection<TItem>(IDatabaseCollectionProvider<TContext> collectionProvider);
        IDatabaseCollection<TItem> GetCollection<TItem>(IDatabaseCollectionProvider<TContext> collectionProvider, string collectionId, Type collectionItemType);
        ICollectionInfo GetCollectionInfo<TItem>();
        ICollectionInfo GetCollectionInfo(Type itemType);
    }
}
