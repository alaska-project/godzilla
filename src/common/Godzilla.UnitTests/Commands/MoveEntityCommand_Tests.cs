using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.UnitTests.Commands
{
    public class MoveEntityCommand_Tests
    {
        public class FakeDerivedEntity : FakeEntity
        { }

        public class FakeEntity
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
