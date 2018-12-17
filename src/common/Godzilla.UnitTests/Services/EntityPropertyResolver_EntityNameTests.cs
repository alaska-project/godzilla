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

            Assert.Equal(entityId.ToString(), entityName);
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

            Assert.Equal(entityId.ToString(), entityName);
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

            Assert.Equal(entityId.ToString(), entityName);
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
