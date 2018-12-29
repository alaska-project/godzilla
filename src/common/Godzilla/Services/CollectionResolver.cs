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
        private readonly SafeDictionary<Type, Type> _baseTypesMap = new SafeDictionary<Type, Type>();
        private readonly SafeDictionary<Type, ICollectionInfo> _collectionMap = new SafeDictionary<Type, ICollectionInfo>();

        public CollectionResolver()
        {
        }

        public IDatabaseCollection<TItem> GetCollection<TItem>(IDatabaseCollectionProvider<TContext> collectionProvider)
        {
            var collectionInfo = GetCollectionInfo<TItem>();

            return GetCollection<TItem>(collectionProvider, collectionInfo.CollectionId, collectionInfo.CollectionItemType);
        }

        public IDatabaseCollection<TItem> GetCollection<TItem>(IDatabaseCollectionProvider<TContext> collectionProvider, string collectionId, Type collectionItemType)
        {
            //collection item type and actual requested item type is the same
            if (collectionItemType == typeof(TItem))
                return collectionProvider.GetCollection<TItem, TItem>(collectionId);

            //requested item type is derived from collection item type
            //TODO: cache
            var getCollectionMethod = ReflectionUtil.GetGenericMethod(collectionProvider.GetType(), "GetCollection", BindingFlags.Instance | BindingFlags.Public, 2, 1);
            var genericGetCollectionMethod = getCollectionMethod.MakeGenericMethod(typeof(TItem), collectionItemType);
            return (IDatabaseCollection<TItem>)genericGetCollectionMethod.Invoke(collectionProvider, new object[] { collectionId });
        }

        public ICollectionInfo GetCollectionInfo<TItem>()
        {
            var collectionType = typeof(TItem);
            return GetCollectionInfo(collectionType);
        }

        public ICollectionInfo GetCollectionInfo(Type itemType)
        {
            return _collectionMap.Retreive(
                itemType,
                () =>
                {
                    var collectionBaseType = GetRootCollectionType(itemType);

                    var collectionId = BuildCollectionId(collectionBaseType);
                    
                    //var overlappingCollection = _collectionMap.Values
                    //    .FirstOrDefault(x => x.CollectionId.Equals(collectionId));

                    //if (overlappingCollection != null)
                    //{
                    //    throw new DuplicateCollectionIdException($"Collection {collectionId} already used by {overlappingCollection.CollectionItemType.FullName}");
                    //}

                    return new CollectionInfo(collectionBaseType, collectionId);
                });
        }

        private string BuildCollectionId(Type itemType)
        {
            var collectionAttribute = GetCollectionAttribute(itemType);
            return !string.IsNullOrEmpty(collectionAttribute?.CollectionId) ?
                collectionAttribute.CollectionId :
                itemType.Name;
        }

        private Type GetRootCollectionType(Type itemType)
        {
            return _baseTypesMap.Retreive(itemType, () =>
            {
                var baseTypes = ReflectionUtil.GetBaseTypesAndSelf(itemType);
                return baseTypes.FirstOrDefault(HasCollectionAttribute) ??
                    baseTypes.Last();
            });
        }

        private bool HasCollectionAttribute(Type type)
        {
            return GetCollectionAttribute(type) != null;
        }

        private CollectionAttribute GetCollectionAttribute(Type type)
        {
            return type.GetCustomAttribute<CollectionAttribute>(false);
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
