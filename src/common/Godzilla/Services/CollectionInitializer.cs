using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Services
{
    internal class CollectionInitializer : ICollectionInitializer
    {
        public GodzillaCollection<TItem> CreateCollection<TItem>(IDatabaseCollection<TItem> collection)
        {
            return new GodzillaCollection<TItem>(collection);
        }

        public TCollection CreateCollection<TItem, TCollection>(IDatabaseCollection<TItem> collection)
            where TCollection : IGodzillaCollection<TItem>
        {
            return (TCollection)Activator.CreateInstance(typeof(TCollection), new object[] { collection });
        }
    }
}
