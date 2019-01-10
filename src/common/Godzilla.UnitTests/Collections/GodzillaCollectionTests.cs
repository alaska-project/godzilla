using Godzilla.Abstractions.Infrastructure;
using Godzilla.Collections.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        public async Task Add_item()
        {
            var newItem = new FakeGodzillaItem();

            await _godzillaCollection.Add(newItem);
            _databaseCollection.Verify(c => c.Add(newItem), Times.Once);
        }

        [Fact]
        public async Task Add_derived_item()
        {
            var newItem = new FakeGodzilleDerivedItem();

            await _godzillaCollection.Add(newItem);
            _databaseCollection.Verify(c => c.Add(newItem), Times.Once);
        }

        [Fact]
        public async Task Delete_item()
        {
            var newItem = new FakeGodzillaItem();

            await _godzillaCollection.Delete(newItem);

            _databaseCollection.Verify(c => c.Delete(newItem), Times.Once);
        }

        [Fact]
        public async Task Delete_derived_item()
        {
            var newItem = new FakeGodzilleDerivedItem();

            await _godzillaCollection.Delete(newItem);
            _databaseCollection.Verify(c => c.Delete(newItem), Times.Once);
        }

        [Fact]
        public async Task Update_item()
        {
            var newItem = new FakeGodzillaItem();

            await _godzillaCollection.Update(newItem);
            _databaseCollection.Verify(c => c.Update(newItem), Times.Once);
        }

        [Fact]
        public async Task Update_derived_item()
        {
            var newItem = new FakeGodzilleDerivedItem();

            await _godzillaCollection.Update(newItem);
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
