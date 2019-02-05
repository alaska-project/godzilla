using Godzilla.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions
{
    public interface IEntityContextServiceCollection<TContext>
        where TContext : EntityContext
    {
        IServiceCollection Services { get; }
    }
}
