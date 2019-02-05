using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions
{
    public interface IGodzillaServiceBuilder
    {
        IServiceCollection Services { get; }
    }
}
