using Godzilla.Abstractions;
using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Godzilla.Services
{
    internal class CollectionService<TContext> :
        ICollectionService<TContext>
        where TContext : EntityContext
    {
        private readonly ICollectionInitializer _initializer;
        private readonly ICollectionResolver<TContext> _resolver;
        private readonly IDatabaseCollectionProvider<TContext> _provider;

        public CollectionService(
            ICollectionInitializer initializer,
            ICollectionResolver<TContext> resolver,
            IDatabaseCollectionProvider<TContext> provider)
        {
            _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public IGodzillaCollection GetCollection(Type itemType)
        {
            var getCollectionMethod = ReflectionUtil.GetGenericMethod(this.GetType(), "GetCollection", BindingFlags.Instance | BindingFlags.Public);
            var specificGetCollectionMethod = getCollectionMethod.MakeGenericMethod(itemType);
            return (IGodzillaCollection)specificGetCollectionMethod.Invoke(this, new object[0]);
        }

        public IGodzillaCollection<TItem> GetCollection<TItem>()
        {
            var collection = _resolver.GetCollection<TItem>(_provider);
            return _initializer.CreateCollection(collection);
        }

        public TCollection GetCollection<TItem, TCollection>() where TCollection : IGodzillaCollection<TItem>
        {
            var collection = _resolver.GetCollection<TItem>(_provider);
            return _initializer.CreateCollection<TItem, TCollection>(collection);
        }
    }
}
