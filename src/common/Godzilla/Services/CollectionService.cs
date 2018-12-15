using Godzilla.Abstractions;
using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Services
{
    internal class CollectionService<TContext> :
        ICollectionService<TContext>
        where TContext : EntityContext
    {
        private readonly ICollectionResolver<TContext> _resolver;
        private readonly IDatabaseCollectionProvider<TContext> _provider;

        public CollectionService(
            ICollectionResolver<TContext> resolver,
            IDatabaseCollectionProvider<TContext> provider)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public IDatabaseCollection<TEntity> GetCollection<TEntity>()
        {
            var collectionInfo = _resolver.GetCollectionInfo<TEntity>();
            return _provider.GetCollection<TEntity>(collectionInfo.CollectionId);
        }
    }
}
