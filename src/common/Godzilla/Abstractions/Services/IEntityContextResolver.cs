using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    public interface IEntityContextResolver
    {
        IEnumerable<EntityContextReference> GetContextReferences();
        EntityContextReference GetContextReference(string contextId);
    }
}
