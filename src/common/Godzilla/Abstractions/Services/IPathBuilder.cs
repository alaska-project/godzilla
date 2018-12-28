using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    internal interface IPathBuilder<TContext>
        where TContext : EntityContext
    {
    }
}
