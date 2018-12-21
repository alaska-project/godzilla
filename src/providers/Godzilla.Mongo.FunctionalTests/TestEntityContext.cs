using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.FunctionalTests
{
    public class TestEntityContext : EntityContext
    {
        public TestEntityContext(IMediator mediator)
            : base(mediator)
        { }
    }

    public class TestEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class DerivedTestEntity : TestEntity
    {
        public string SecondName { get; set; }
    }
}
