using Godzilla.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Internal
{
    internal class EntityContextServiceCollection<TContext> : IEntityContextServiceCollection<TContext>
        where TContext : EntityContext
    {
        public EntityContextServiceCollection(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
