using Godzilla.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Godzilla.DemoWebApp.Application
{
    public class DemoEntityContext : EntityContext<DemoEntityContext>
    {
        public DemoEntityContext(IEntityContextServices<DemoEntityContext> services)
            : base(services)
        { }
    }
}
