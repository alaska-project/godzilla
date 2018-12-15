using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions
{
    public interface IGodzillaOptions<TContext>
        where TContext : EntityContext
    {
        string PathSeparator { get; }
    }
}
