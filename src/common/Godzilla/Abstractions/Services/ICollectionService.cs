using Godzilla.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    public interface ICollectionService<TContext> : ICollectionService
        where TContext : EntityContext
    { }

    public interface ICollectionService
    {
        IDatabaseCollection GetRawCollection(string collectionId);
        IGodzillaCollection GetCollection(Type itemType);
        IGodzillaCollection<TItem> GetCollection<TItem>();
        TCollection GetCollection<TItem, TCollection>() where TCollection : IGodzillaCollection<TItem>;
    }
}
