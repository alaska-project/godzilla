using Godzilla.Exceptions;
using Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Godzilla.UnitTests.Services
{
    public class EntityPropertyResolver_EntityNameTests
    {
        [Fact]
        public void Generate_id_hash()
        {
            var resolver = new EntityPropertyResolver<FakeEntityContext>();

            var hash = resolver.GenerateIdHash(new Guid("ef6f8868-6f09-4f54-b23f-5e616cca4f1c"));
        }

        [Fact]
        public void Missing_name_property_and_with_empty_id()
        {
            var resolver = new EntityPropertyResolver<FakeEntityContext>();

            Assert.Throws<MissingIdException>(
                () => resolver.GetEntityName(new EntityWithoutName()));
        }

        [Fact]
        public void Missing_name_property_with_id_not_empty()
        {
            var resolver = new EntityPropertyResolver<FakeEntityContext>();

            var entityId = Guid.NewGuid();
            var entityName = resolver.GetEntityName(new EntityWithoutName
            {
                Id = entityId
            });

            var hash = resolver.GenerateIdHash(entityId);
            Assert.Equal(hash, entityName);
        }

        [Fact]
        public void Null_name_property_with_id_not_empty()
        {
            var resolver = new EntityPropertyResolver<FakeEntityContext>();

            var entityId = Guid.NewGuid();
            var entityName = resolver.GetEntityName(new EntityWithStringName
            {
                Name = null,
                Id = entityId
            });

            var hash = resolver.GenerateIdHash(entityId);
            Assert.Equal(hash, entityName);
        }

        [Fact]
        public void Empty_name_property_with_id_not_empty()
        {
            var resolver = new EntityPropertyResolver<FakeEntityContext>();

            var entityId = Guid.NewGuid();
            var entityName = resolver.GetEntityName(new EntityWithStringName
            {
                Name = string.Empty,
                Id = entityId
            });

            var hash = resolver.GenerateIdHash(entityId);
            Assert.Equal(hash, entityName);
        }

        public class EntityWithoutName
        {
            public Guid Id { get; set; }
        }

        public class EntityWithStringName
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
