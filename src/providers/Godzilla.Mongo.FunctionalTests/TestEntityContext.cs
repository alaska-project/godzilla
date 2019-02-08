using Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.FunctionalTests
{
    public class TestEntityContext : EntityContext<TestEntityContext>
    {
        public TestEntityContext(IEntityContextServices<TestEntityContext> services)
            : base(services)
        { }
    }

    public class TestEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Val2 { get; set; }
    }

    public class DerivedTestEntity : TestEntity
    {
        public string SecondName { get; set; }
    }

    public class OtherTestEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
