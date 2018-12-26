using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.Commands;
using Godzilla.DomainModels;
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
    public class UpdateEntitiesCommand_Tests
    {
        private Mock<ITransactionService<FakeEntityContext>> _transactionService = new Mock<ITransactionService<FakeEntityContext>>();
        private Mock<IEntityPropertyResolver<FakeEntityContext>> _propertyResolver = new Mock<IEntityPropertyResolver<FakeEntityContext>>();
        private Mock<IEntityCommandsHelper<FakeEntityContext>> _commandsHelper = new Mock<IEntityCommandsHelper<FakeEntityContext>>();
        private Mock<TreeEdgesCollection> _treeEdgesCollection = new Mock<TreeEdgesCollection>();
        private Mock<IGodzillaCollection> _entityCollection = new Mock<IGodzillaCollection>();
        private Mock<IGodzillaCollection> _derivedEntityCollection = new Mock<IGodzillaCollection>();

        public UpdateEntitiesCommand_Tests()
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
        public async Task Update_entity_ok()
        {
            var request = FakeUpdateEntityCommand();
            var handler = new UpdateEntitiesCommandHandler<FakeEntityContext>(_transactionService.Object, _commandsHelper.Object);

            await handler.Handle(request, default(CancellationToken));
        }

        [Fact]
        public async Task Update_derived_entity_ok()
        {
            var request = FakeUpdateDerivedEntityCommand();
            var handler = new UpdateEntitiesCommandHandler<FakeEntityContext>(_transactionService.Object, _commandsHelper.Object);

            await handler.Handle(request, default(CancellationToken));
        }

        [Fact]
        public void Update_non_existing_entity_ko()
        { }

        private UpdateEntitiesCommand<FakeEntityContext> FakeUpdateEntityCommand()
        {
            return new Godzilla.Commands.UpdateEntitiesCommand<FakeEntityContext>(new List<FakeEntity> {
                new FakeEntity
                {
                    Id = Guid.NewGuid(),
                }
            });
        }

        private UpdateEntitiesCommand<FakeEntityContext> FakeUpdateDerivedEntityCommand()
        {
            return new Godzilla.Commands.UpdateEntitiesCommand<FakeEntityContext>(new List<FakeDerivedEntity> {
                new FakeDerivedEntity
                {
                    Id = Guid.NewGuid(),
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
