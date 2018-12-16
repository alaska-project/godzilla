using Godzilla.Exceptions;
using Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Godzilla.UnitTests.Services
{
    public class EntityPropertyResolver_EntityIdTests
    {
        [Fact]
        public void Missing_entity_property_id()
        {
            var resolver = new EntityPropertyResolver<FakeEntityContext>();

            Assert.Throws<MissingIdPropertyException>(
                () => resolver.GetEntityId(new EntityWithoutIdProperty())
                );
        }

        [Fact]
        public void Invalid_entity_property_id_type()
        {
            var resolver = new EntityPropertyResolver<FakeEntityContext>();

            Assert.Throws<WrongIdPropertyTypeException>(
                () => resolver.GetEntityId(new EntityWithWrongIdType())
                );
        }

        [Fact]
        public void Valid_id_property_but_empty_id()
        {
            var resolver = new EntityPropertyResolver<FakeEntityContext>();

            var newId = resolver.GetEntityId(new EntityWithCorrectIdType(), true);

            Assert.NotEqual(Guid.Empty, newId);
        }

        [Fact]
        public void Valid_id_property_and_id_not_empty()
        {
            var resolver = new EntityPropertyResolver<FakeEntityContext>();

            var id = Guid.NewGuid();
            var entityId = resolver.GetEntityId(new EntityWithCorrectIdType
            {
                Id = id
            });

            Assert.Equal(id, entityId);
        }


    }

    public class EntityWithoutIdProperty
    {
        public string Idd { get; set; }
    }

    public class EntityWithWrongIdType
    {
        public string Id { get; set; } 
    }

    public class EntityWithCorrectIdType
    {
        public Guid Id { get; set; }
    }
}
