using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Services
{
    internal class EntityCollectionsService<TContext> : IEntityCollectionsService<TContext>
        where TContext : EntityContext
    {
        private readonly ICollectionService<TContext> _collectionService;

        public EntityCollectionsService(ICollectionService<TContext> collectionService)
        {
            _collectionService = collectionService ?? throw new ArgumentNullException(nameof(collectionService));
        }

        public IGodzillaCollection<TEntity> GetCollection<TEntity>()
        {
            var collection = _collectionService.GetCollection<TEntity>();
            if (collection == null)
                throw new CollectionNotFoundException($"No collections found for type {typeof(TEntity).FullName}");

            return collection;
        }

        public EntityNodesCollection GetEntityNodesCollection()
        {
            return _collectionService.GetCollection<EntityNode, EntityNodesCollection>();
        }
    }
}
