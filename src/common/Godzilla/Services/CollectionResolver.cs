using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using Godzilla.Attributes;
using Godzilla.Exceptions;
using Godzilla.Utils;
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
        private readonly SafeDictionary<Type, ICollectionInfo> _collectionMap = new SafeDictionary<Type, ICollectionInfo>();
        private readonly IDatabaseCollectionProvider<TContext> _collectionProvider;

        public CollectionResolver(IDatabaseCollectionProvider<TContext> collectionProvider)
        {
            _collectionProvider = collectionProvider ?? throw new ArgumentNullException(nameof(collectionProvider));
        }

        public ICollectionInfo GetCollectionInfo<TItem>()
        {
            var collectionType = typeof(TItem);

            return _collectionMap.Retreive(
                collectionType, 
                () => 
                {
                    var collectionId = BuildCollectionId<TItem>();
                    var overlappedCollection = _collectionMap.Values
                        .FirstOrDefault(x => x.CollectionId.Equals(collectionId));
                    if (overlappedCollection != null)
                    {
                        throw new DuplicateCollectionIdException($"Collection {collectionId} already used by {overlappedCollection.CollectionItemType.FullName}");
                    }

                    return new CollectionInfo(collectionType, collectionId);
                });
        }

        private string BuildCollectionId<TItem>()
        {
            return typeof(TItem).Name;
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
