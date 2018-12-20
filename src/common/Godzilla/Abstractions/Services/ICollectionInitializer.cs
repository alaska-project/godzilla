using Godzilla.Abstractions.Infrastructure;
using Godzilla.Collections.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    internal interface ICollectionInitializer
    {
        GodzillaCollection<TItem> CreateCollection<TItem>(IDatabaseCollection<TItem> collection);
        TCollection CreateCollection<TItem, TCollection>(IDatabaseCollection<TItem> collection) where TCollection : IGodzillaCollection<TItem>;
    }
}
