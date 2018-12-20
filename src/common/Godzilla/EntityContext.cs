using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Godzilla.UnitTests")]
namespace Godzilla
{
    public abstract class EntityContext
    {
        public virtual void OnConfiguring()
        { }
    }
}
