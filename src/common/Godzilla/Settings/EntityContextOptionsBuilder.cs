using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public class EntityContextOptionsBuilder
    {
        private readonly IServiceCollection services;

        internal EntityContextOptionsBuilder(IServiceCollection services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        } 
    }
}
