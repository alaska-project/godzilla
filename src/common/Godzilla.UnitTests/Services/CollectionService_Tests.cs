using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.UnitTests.Services
{
    public class CollectionService_Tests
    {
        private readonly Mock<ICollectionInitializer> _initializer = new Mock<ICollectionInitializer>();
        private readonly Mock<ICollectionResolver<FakeEntityContext>> _resolver = new Mock<ICollectionResolver<FakeEntityContext>>();
        private readonly Mock<IDatabaseCollectionProvider<FakeEntityContext>> _provider = new Mock<IDatabaseCollectionProvider<FakeEntityContext>>();

        public CollectionService_Tests()
        {

        }
    }
}
