using Godzilla.Abstractions.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Godzilla.UnitTests.Commands
{
    public class UpdateEntitiesCommand_Tests
    {
        private Mock<ITransactionService<FakeEntityContext>> _transactionService = new Mock<ITransactionService<FakeEntityContext>>();
        private Mock<IEntityPropertyResolver<FakeEntityContext>> _propertyResolver = new Mock<IEntityPropertyResolver<FakeEntityContext>>();
        private Mock<IGodzillaCollection> _entityCollection = new Mock<IGodzillaCollection>();
        private Mock<IGodzillaCollection> _derivedEntityCollection = new Mock<IGodzillaCollection>();

        [Fact]
        public void Update_entity_ok()
        {
        }

        [Fact]
        public void Update_derived_entity_ok()
        { }

        [Fact]
        public void Update_non_existing_entity_ko()
        { }

    }
}
