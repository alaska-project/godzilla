//using Godzilla.Abstractions.Services;
//using Godzilla.Collections.Internal;
//using Godzilla.Commands;
//using Godzilla.DomainModels;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace Godzilla.UnitTests.Commands
//{
//    public class DeleteEntitiesCommand_Tests
//    {
//        private Mock<ITransactionService<FakeEntityContext>> _transactionService = new Mock<ITransactionService<FakeEntityContext>>();
//        private Mock<IEntityCommandsHelper<FakeEntityContext>> _commandsHelper = new Mock<IEntityCommandsHelper<FakeEntityContext>>();
//        private Mock<IEntityPropertyResolver<FakeEntityContext>> _propertyResolver = new Mock<IEntityPropertyResolver<FakeEntityContext>>();
//        private Mock<TreeEdgesCollection> _treeEdgesCollection = new Mock<TreeEdgesCollection>();
//        private Mock<IGodzillaCollection> _entityCollection = new Mock<IGodzillaCollection>();
//        private Mock<IGodzillaCollection> _derivedEntityCollection = new Mock<IGodzillaCollection>();

//        public DeleteEntitiesCommand_Tests()
//        {
//            _propertyResolver
//                .Setup(x => x.GetEntityId(It.IsAny<FakeEntity>()))
//                .Returns((FakeEntity t) => t.Id);

//            _propertyResolver
//                .Setup(x => x.GetEntityId(It.IsAny<FakeEntity>(), It.IsAny<bool>()))
//                .Returns((FakeEntity t, bool g) => t.Id == Guid.Empty && g ? Guid.NewGuid() : t.Id);

//            _transactionService
//                .Setup(x => x.GetCollection<TreeEdge, TreeEdgesCollection>())
//                .Returns(_treeEdgesCollection.Object);

//            _transactionService
//                .Setup(x => x.GetCollection(typeof(FakeEntity)))
//                .Returns(_entityCollection.Object);

//            _transactionService
//                .Setup(x => x.GetCollection(typeof(FakeDerivedEntity)))
//                .Returns(_derivedEntityCollection.Object);

//            _commandsHelper
//                .Setup(x => x.GetEntityType(It.IsAny<IEnumerable<object>>()))
//                .Returns((IEnumerable<object> entities) => entities.First().GetType());

//            _commandsHelper
//                .Setup(x => x.GetEntitiesId(It.IsAny<IEnumerable<FakeEntity>>()))
//                .Returns((IEnumerable<FakeEntity> entities) => entities.Select(x => x.Id));

//        }

//        [Fact]
//        public async Task Delete_entity_ok()
//        {
//            //setup
//            var entityId = Guid.NewGuid();
//            var request = FakeDeleteEntityCommand(entityId);
//            var handler = new DeleteEntitiesCommandHandler<FakeEntityContext>(_transactionService.Object, _commandsHelper.Object);

//            _commandsHelper
//                .Setup(x => x.VerifyEntitiesExist(It.IsAny<IEnumerable<Guid>>(), It.IsAny<TreeEdgesCollection>()))
//                .Returns(request.Entities);

//            //run
//            await handler.Handle(request, default(CancellationToken));
//        }

//        private DeleteEntitiesCommand<FakeEntityContext> FakeDeleteEntityCommand(Guid entityId)
//        {
//            return new Godzilla.Commands.DeleteEntitiesCommand<FakeEntityContext>(new List<FakeEntity> {
//                new FakeEntity
//                {
//                    Id = entityId,
//                }
//            });
//        }

//        public class FakeDerivedEntity : FakeEntity
//        { }

//        public class FakeEntity
//        {
//            public Guid Id { get; set; }
//            public string Name { get; set; }
//        }
//    }
//}
