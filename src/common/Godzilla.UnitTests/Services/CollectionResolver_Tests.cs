using Godzilla.Abstractions.Infrastructure;
using Godzilla.Attributes;
using Godzilla.Exceptions;
using Godzilla.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Godzilla.UnitTests.Services
{
    public class CollectionResolver_Tests
    {
        private CollectionResolver<FakeEntityContext> _resolver = new CollectionResolver<FakeEntityContext>();
        private Mock<IDatabaseCollectionProvider<FakeEntityContext>> _collectionProvider = new Mock<IDatabaseCollectionProvider<FakeEntityContext>>();

        [Fact]
        public void Resolve_base_type_collection()
        {
            _resolver.GetCollection<RootType>(_collectionProvider.Object);
        }

        [Fact]
        public void Resolve_derived_collection()
        {
            _resolver.GetCollection<MiddleType>(_collectionProvider.Object);
        }

        [Fact]
        public void Resolve_base_type_from_self()
        {
            var collectionInfo = _resolver.GetCollectionInfo<RootType>();
            Assert.Equal(typeof(RootType), collectionInfo.CollectionItemType);
            Assert.Equal(typeof(RootType).Name, collectionInfo.CollectionId);
        }

        [Fact]
        public void Resolve_base_type_from_middle_type()
        {
            var collectionInfo = _resolver.GetCollectionInfo<MiddleType>();
            Assert.Equal(typeof(RootType), collectionInfo.CollectionItemType);
            Assert.Equal(typeof(RootType).Name, collectionInfo.CollectionId);
        }

        [Fact]
        public void Resolve_derived_type_with_custom_attribute_from_self()
        {
            var collectionInfo = _resolver.GetCollectionInfo<DerivedTypeWithCollectionAttribute>();
            Assert.Equal(typeof(DerivedTypeWithCollectionAttribute), collectionInfo.CollectionItemType);
            Assert.Equal(typeof(DerivedTypeWithCollectionAttribute).Name, collectionInfo.CollectionId);
        }

        [Fact]
        public void Resolve_derived_type_with_custom_attribute_from_leaf()
        {
            var collectionInfo = _resolver.GetCollectionInfo<LeafTypeFromCollectionAttributeType>();
            Assert.Equal(typeof(DerivedTypeWithCollectionAttribute), collectionInfo.CollectionItemType);
            Assert.Equal(typeof(DerivedTypeWithCollectionAttribute).Name, collectionInfo.CollectionId);
        }

        [Fact]
        public void Resolve_derived_type_with_custom_attribute_with_custom_name_from_self()
        {
            var collectionInfo = _resolver.GetCollectionInfo<DerivedTypeWithCollectionAttributeWithCustomName>();
            Assert.Equal(typeof(DerivedTypeWithCollectionAttributeWithCustomName), collectionInfo.CollectionItemType);
            Assert.Equal("custom-name", collectionInfo.CollectionId);
        }

        [Fact]
        public void Resolve_derived_type_with_custom_attribute_with_custom_name_from_leaf()
        {
            var collectionInfo = _resolver.GetCollectionInfo<LeafTypeFromCollectionAttributeTypeWithCustomName>();
            Assert.Equal(typeof(DerivedTypeWithCollectionAttributeWithCustomName), collectionInfo.CollectionItemType);
            Assert.Equal("custom-name", collectionInfo.CollectionId);
        }

        [Fact]
        public void Resolve_derived_type_with_overlapping_collection_id()
        {
            _resolver.GetCollectionInfo<LeafTypeFromCollectionAttributeTypeWithCustomName>();

            Assert.Throws<DuplicateCollectionIdException>(() => 
            {
                _resolver.GetCollectionInfo<DerivedTypeWithOverlappngCollectionName>();
            });
        }

        [Fact]
        public void Resolve_derived_type_with_no_overlapping_collection_id_multiple_times_without_exceptions()
        {
            _resolver.GetCollectionInfo<LeafTypeFromCollectionAttributeTypeWithCustomName>();
            _resolver.GetCollectionInfo<LeafTypeFromCollectionAttributeTypeWithCustomName>();
            _resolver.GetCollectionInfo<LeafTypeFromCollectionAttributeTypeWithCustomName>();
            _resolver.GetCollectionInfo<LeafTypeFromCollectionAttributeTypeWithCustomName>();
        }

        public class RootType
        { }

        public class MiddleType : RootType
        { }

        [Attributes.Collection]
        public class DerivedTypeWithCollectionAttribute : MiddleType
        { }

        public class LeafTypeFromCollectionAttributeType : DerivedTypeWithCollectionAttribute
        { }

        [Attributes.Collection("custom-name")]
        public class DerivedTypeWithCollectionAttributeWithCustomName : MiddleType
        { }

        public class LeafTypeFromCollectionAttributeTypeWithCustomName : DerivedTypeWithCollectionAttributeWithCustomName
        { }

        [Attributes.Collection("custom-name")]
        public class DerivedTypeWithOverlappngCollectionName : MiddleType
        { }
    }
}
