using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    public interface IEntityContextInitializer
    {
        void Initialize(EntityContext context, IEntityConfigurator configurator);
    }
}
