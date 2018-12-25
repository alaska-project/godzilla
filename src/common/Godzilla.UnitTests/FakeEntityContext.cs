using Godzilla.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.UnitTests
{
    public class FakeEntityContext : EntityContext<FakeEntityContext>
    {
        public FakeEntityContext(IEntityContextServices<FakeEntityContext> services)
            : base(services)
        {}
    }
}
