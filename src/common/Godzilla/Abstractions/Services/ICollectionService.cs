using Godzilla.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    internal interface ICollectionService<TContext>
        where TContext : EntityContext
    {
        IGodzillaCollection GetCollection(Type itemType);
        IGodzillaCollection<TItem> GetCollection<TItem>();
        TCollection GetCollection<TItem, TCollection>() where TCollection : IGodzillaCollection<TItem>;
    }
}
