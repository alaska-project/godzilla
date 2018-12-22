using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.UnitTests
{
    public class FakeEntityContext : EntityContext
    {
        public FakeEntityContext(IMediator mediator)
            : base(mediator)
        {}
    }
}
