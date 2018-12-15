using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using Godzilla.Attributes;
using Godzilla.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Godzilla.Services
{
    internal class CollectionResolver<TContext> :
        ICollectionResolver<TContext>
        where TContext : EntityContext
    {
        private readonly Dictionary<Type, ICollectionInfo> _collectionMap = new Dictionary<Type, ICollectionInfo>();
        private readonly IDatabaseCollectionProvider<TContext> _collectionProvider;

        public CollectionResolver(IDatabaseCollectionProvider<TContext> collectionProvider)
        {
            _collectionProvider = collectionProvider ?? throw new ArgumentNullException(nameof(collectionProvider));
        }

        public ICollectionInfo GetCollectionInfo<TEntity>()
        {
            var collectionType = typeof(TEntity);
            if (_collectionMap.ContainsKey(collectionType))
                return _collectionMap[collectionType];

            lock (this)
            {
                //check if it has been initialized by any other process in the meanwhile
                if (_collectionMap.ContainsKey(collectionType))
                    return _collectionMap[collectionType];

                var collectionId = BuildCollectionId<TEntity>();
                var overlappedCollection = _collectionMap.Values
                    .FirstOrDefault(x => x.CollectionId.Equals(collectionId));
                if (overlappedCollection != null)
                {
                    throw new DuplicateCollectionIdException($"Collection {collectionId} already used by {overlappedCollection.CollectionItemType.FullName}");
                }

                var collectionInfo = new CollectionInfo(collectionType, collectionId);
                _collectionMap.Add(collectionType, collectionInfo);
                return collectionInfo;
            }
        }

        private string BuildCollectionId<TEntity>()
        {
            return typeof(TEntity).Name;
        }

        private CollectionAttribute GetCollectionAttribute(Type type)
        {
            return type.GetCustomAttribute<CollectionAttribute>(true);
        }
    }

    public class CollectionInfo : ICollectionInfo
    {
        public CollectionInfo(Type collectionItemType, string collectionId)
        {
            CollectionItemType = collectionItemType;
            CollectionId = collectionId;
        }

        public Type CollectionItemType { get; }
        public string CollectionId { get; }
    }
}
