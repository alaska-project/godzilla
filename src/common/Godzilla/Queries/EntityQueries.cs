using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Queries
{
    internal class EntityQueries<TContext> : IEntityQueries
        where TContext : EntityContext
    {
        private readonly ICollectionService<TContext> _collectionService;

        public EntityQueries(ICollectionService<TContext> collectionService)
        {
            _collectionService = collectionService ?? throw new ArgumentNullException(nameof(collectionService));
        }

        public IQueryable<TEntity> AsQueryable<TEntity>()
        {
            var collection = _collectionService.GetCollection<TEntity>();
            if (collection == null)
                throw new CollectionNotFoundException($"No collections found for type {typeof(TEntity).FullName}");

            return collection.AsQueryable();
        }
    }
}
