using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.Commands;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Godzilla.UnitTests.Commands
{
    public class CreateEntitiesCommand_Tests
    {
        private Mock<ITransactionService<FakeEntityContext>> _transactionService = new Mock<ITransactionService<FakeEntityContext>>();
        private Mock<IEntityCommandsHelper<FakeEntityContext>> _commandsHelper = new Mock<IEntityCommandsHelper<FakeEntityContext>>();
        private Mock<IEntityPropertyResolver<FakeEntityContext>> _propertyResolver = new Mock<IEntityPropertyResolver<FakeEntityContext>>();
        private Mock<TreeEdgesCollection> _treeEdgesCollection = new Mock<TreeEdgesCollection>();
        private Mock<IGodzillaCollection> _entityCollection = new Mock<IGodzillaCollection>();
        private Mock<IGodzillaCollection> _derivedEntityCollection = new Mock<IGodzillaCollection>();

        public CreateEntitiesCommand_Tests()
        {
            _propertyResolver
                .Setup(x => x.GetEntityId(It.IsAny<FakeEntity>()))
                .Returns((FakeEntity t) => t.Id);

            _propertyResolver
                .Setup(x => x.GetEntityId(It.IsAny<FakeEntity>(), It.IsAny<bool>()))
                .Returns((FakeEntity t, bool g) => t.Id == Guid.Empty && g ? Guid.NewGuid() : t.Id);

            _transactionService
                .Setup(x => x.GetCollection<TreeEdge, TreeEdgesCollection>())
                .Returns(_treeEdgesCollection.Object);

            _transactionService
                .Setup(x => x.GetCollection(typeof(FakeEntity)))
                .Returns(_entityCollection.Object);

            _transactionService
                .Setup(x => x.GetCollection(typeof(FakeDerivedEntity)))
                .Returns(_derivedEntityCollection.Object);

            _commandsHelper
                .Setup(x => x.GetEntityType(It.IsAny<IEnumerable<object>>()))
                .Returns((IEnumerable<object> entities) => entities.First().GetType());
        }

        [Fact]
        public async Task Create_derived_entity_ok()
        {
            //setup
            var entityId = Guid.NewGuid();
            var request = FakeCreateDerivedEntityCommand(Guid.Empty, entityId);
            var handler = new CreateEntitiesCommandHandler<FakeEntityContext>(_transactionService.Object, _propertyResolver.Object, _commandsHelper.Object);

            _treeEdgesCollection
                .Setup(x => x.NodeExists(It.IsAny<Guid>()))
                .Returns(false);

            _treeEdgesCollection
                .Setup(x => x.GetNodes(It.IsAny<IEnumerable<Guid>>()))
                .Returns(new List<TreeEdge>());

            //act
            await handler.Handle(request, default(CancellationToken));

            _transactionService.Verify(x => x.StartTransaction(), Times.Once());
            _transactionService.Verify(x => x.CommitTransaction(), Times.Once());
            _transactionService.Verify(x => x.AbortTransaction(), Times.Never());

            _treeEdgesCollection.Verify(x => x.Add(It.Is<IEnumerable<TreeEdge>>(t =>
                t.First().ParentId == request.ParentId &&
                t.First().NodeId == entityId)), Times.Once());
            _entityCollection.Verify(x => x.Add(It.IsAny<object>()), Times.Never());
            _derivedEntityCollection.Verify(x => x.Add(request.Entities), Times.Once());
        }

        [Fact]
        public async Task Create_duplicate_derived_entity()
        {
            //setup
            var entityId = Guid.NewGuid();
            var request = FakeCreateDerivedEntityCommand(Guid.Empty, entityId);
            var handler = new CreateEntitiesCommandHandler<FakeEntityContext>(_transactionService.Object, _propertyResolver.Object, _commandsHelper.Object);

            _treeEdgesCollection
                .Setup(x => x.NodeExists(It.IsAny<Guid>()))
                .Returns(true);

            _treeEdgesCollection
                .Setup(x => x.GetNodes(It.IsAny<IEnumerable<Guid>>()))
                .Returns(new List<TreeEdge> { new TreeEdge() });
            
            //act
            await Assert.ThrowsAsync<EntitiesCreationException>(() => handler.Handle(request, default(CancellationToken)));

            //verify
            _transactionService.Verify(x => x.StartTransaction(), Times.Once());
            _transactionService.Verify(x => x.CommitTransaction(), Times.Never());
            _transactionService.Verify(x => x.AbortTransaction(), Times.Once());

            _treeEdgesCollection.Verify(x => x.Add(It.IsAny<TreeEdge>()), Times.Never());
            _derivedEntityCollection.Verify(x => x.Add(It.IsAny<object>()), Times.Never());
            _entityCollection.Verify(x => x.Add(It.IsAny<object>()), Times.Never());
        }

        [Fact]
        public async Task Create_duplicate_entity()
        {
            //setup
            var entityId = Guid.NewGuid();
            var request = FakeCreateEntityCommand(Guid.Empty, entityId);
            var handler = new CreateEntitiesCommandHandler<FakeEntityContext>(_transactionService.Object, _propertyResolver.Object, _commandsHelper.Object);

            _treeEdgesCollection
                .Setup(x => x.NodeExists(It.IsAny<Guid>()))
                .Returns(true);

            _treeEdgesCollection
                .Setup(x => x.GetNodes(It.IsAny<IEnumerable<Guid>>()))
                .Returns(new List<TreeEdge> { new TreeEdge() });

            //act
            await Assert.ThrowsAsync<EntitiesCreationException>(() => handler.Handle(request, default(CancellationToken)));

            //verify
            _transactionService.Verify(x => x.StartTransaction(), Times.Once());
            _transactionService.Verify(x => x.CommitTransaction(), Times.Never());
            _transactionService.Verify(x => x.AbortTransaction(), Times.Once());

            _treeEdgesCollection.Verify(x => x.Add(It.IsAny<TreeEdge>()), Times.Never());
            _entityCollection.Verify(x => x.Add(It.IsAny<object>()), Times.Never());
        }

        [Fact]
        public async Task Create_entity_ok()
        {
            //setup
            var entityId = Guid.NewGuid();
            var request = FakeCreateEntityCommand(Guid.Empty, entityId);
            var handler = new CreateEntitiesCommandHandler<FakeEntityContext>(_transactionService.Object, _propertyResolver.Object, _commandsHelper.Object);

            _treeEdgesCollection
                .Setup(x => x.NodeExists(It.IsAny<Guid>()))
                .Returns(false);

            _treeEdgesCollection
                .Setup(x => x.GetNodes(It.IsAny<IEnumerable<Guid>>()))
                .Returns(new List<TreeEdge>());

            //act
            await handler.Handle(request, default(CancellationToken));

            _transactionService.Verify(x => x.StartTransaction(), Times.Once());
            _transactionService.Verify(x => x.CommitTransaction(), Times.Once());
            _transactionService.Verify(x => x.AbortTransaction(), Times.Never());

            _treeEdgesCollection.Verify(x => x.Add(It.Is<IEnumerable<TreeEdge>>(t =>
                t.First().ParentId == request.ParentId &&
                t.First().NodeId == entityId)), Times.Once());
            _entityCollection.Verify(x => x.Add(request.Entities), Times.Once());
        }

        [Fact]
        public async Task Create_entity_ok_with_empty_enityId()
        {
            //setup
            var entityId = Guid.Empty;
            var request = FakeCreateEntityCommand(Guid.Empty, entityId);
            var handler = new CreateEntitiesCommandHandler<FakeEntityContext>(_transactionService.Object, _propertyResolver.Object, _commandsHelper.Object);

            _treeEdgesCollection
                .Setup(x => x.NodeExists(It.IsAny<Guid>()))
                .Returns(false);

            _treeEdgesCollection
                .Setup(x => x.GetNodes(It.IsAny<IEnumerable<Guid>>()))
                .Returns(new List<TreeEdge>());

            //act
            await handler.Handle(request, default(CancellationToken));

            _transactionService.Verify(x => x.StartTransaction(), Times.Once());
            _transactionService.Verify(x => x.CommitTransaction(), Times.Once());
            _transactionService.Verify(x => x.AbortTransaction(), Times.Never());

            _treeEdgesCollection.Verify(x => x.Add(It.Is<IEnumerable<TreeEdge>>(t =>
                t.First().ParentId == request.ParentId &&
                t.First().NodeId != Guid.Empty)), Times.Once());
            _entityCollection.Verify(x => x.Add(request.Entities), Times.Once());
        }

        private CreateEntitiesCommand<FakeEntityContext> FakeCreateDerivedEntityCommand(Guid parentId, Guid entityId)
        {
            return new Godzilla.Commands.CreateEntitiesCommand<FakeEntityContext>(parentId, new List<FakeDerivedEntity> {
                new FakeDerivedEntity
                {
                    Id = entityId,
                }
            });
        }

        private CreateEntitiesCommand<FakeEntityContext> FakeCreateEntityCommand(Guid parentId, Guid entityId)
        {
            return new Godzilla.Commands.CreateEntitiesCommand<FakeEntityContext>(parentId, new List<FakeEntity> {
                new FakeEntity
                {
                    Id = entityId,
                }
            });
        }

        public class FakeDerivedEntity : FakeEntity
        { }

        public class FakeEntity
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
