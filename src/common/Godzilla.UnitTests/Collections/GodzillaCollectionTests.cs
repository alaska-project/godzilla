using Godzilla.Abstractions.Infrastructure;
using Godzilla.Collections.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Godzilla.UnitTests.Collections
{
    public class GodzillaCollectionTests
    {
        private readonly Mock<IDatabaseCollection<FakeGodzillaItem>> _databaseCollection = new Mock<IDatabaseCollection<FakeGodzillaItem>>();
        private readonly GodzillaCollection<FakeGodzillaItem> _godzillaCollection;

        public GodzillaCollectionTests()
        {
            _godzillaCollection = new GodzillaCollection<FakeGodzillaItem>(_databaseCollection.Object);
        }

        [Fact]
        public void Add_item()
        {
            var newItem = new FakeGodzillaItem();

            _godzillaCollection.Add(newItem);
            _databaseCollection.Verify(c => c.Add(newItem), Times.Once);
        }

        [Fact]
        public void Add_derived_item()
        {
            var newItem = new FakeGodzilleDerivedItem();

            _godzillaCollection.Add(newItem);
            _databaseCollection.Verify(c => c.Add(newItem), Times.Once);
        }

        [Fact]
        public void Delete_item()
        {
            var newItem = new FakeGodzillaItem();

            _godzillaCollection.Delete(newItem);
            _databaseCollection.Verify(c => c.Delete(newItem), Times.Once);
        }

        [Fact]
        public void Delete_derived_item()
        {
            var newItem = new FakeGodzilleDerivedItem();

            _godzillaCollection.Delete(newItem);
            _databaseCollection.Verify(c => c.Delete(newItem), Times.Once);
        }

        [Fact]
        public void Update_item()
        {
            var newItem = new FakeGodzillaItem();

            _godzillaCollection.Update(newItem);
            _databaseCollection.Verify(c => c.Update(newItem), Times.Once);
        }

        [Fact]
        public void Update_derived_item()
        {
            var newItem = new FakeGodzilleDerivedItem();

            _godzillaCollection.Update(newItem);
            _databaseCollection.Verify(c => c.Update(newItem), Times.Once);
        }
    }

    public class FakeGodzillaItem
    {
        public Guid Id { get; set; }
    }

    public class FakeGodzilleDerivedItem : FakeGodzillaItem
    { }
}
