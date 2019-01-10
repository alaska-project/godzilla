using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using Godzilla.Collections.Infrastructure;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Godzilla.UnitTests.Collections
{
    public class TreeEdgesCollection_QueryableTests
    {
        private readonly Mock<IEntityPropertyResolver<FakeEntityContext>> _propertyResolver = new Mock<IEntityPropertyResolver<FakeEntityContext>>();
        private readonly InMemoryCollection<FakeEntityContext, TreeEdge> _inMemoryCollection;
        private readonly TreeEdgesCollection _treeEdgesCollection;

        public TreeEdgesCollection_QueryableTests()
        {
            _propertyResolver
                .Setup(x => x.GetEntityId(It.IsAny<TreeEdge>()))
                .Returns((TreeEdge t) => t.Id);

            _inMemoryCollection = new InMemoryCollection<FakeEntityContext, TreeEdge>(_propertyResolver.Object, "fake-entity-context");
            _treeEdgesCollection = new TreeEdgesCollection(_inMemoryCollection);
        }

        [Fact]
        public async Task Node_already_exists()
        {
            var node = new TreeEdge
            {
                NodeId = Guid.NewGuid()
            };

            await _treeEdgesCollection.Add(node);
            var nodeExists = _treeEdgesCollection.NodeExists(node.NodeId);
            Assert.True(nodeExists);
        }

        [Fact]
        public void Node_does_not_exists()
        {
            var nodeExists = _treeEdgesCollection.NodeExists(Guid.NewGuid());
            Assert.False(nodeExists);
        }
    }
}
