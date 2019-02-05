using Godzilla.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public class EntityContextOptionsBuilder<TContext>
        where TContext : EntityContext
    {
        internal EntityContextOptionsBuilder(IGodzillaServiceBuilder builder)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public IGodzillaServiceBuilder Builder { get; }
    }
}
