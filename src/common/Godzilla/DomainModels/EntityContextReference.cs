using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.DomainModels
{
    public class EntityContextReference
    {
        public EntityContextReference(string contextId, Type contextType)
        {
            ContextId = contextId;
            ContextType = contextType;
        }

        public string ContextId { get; }
        public Type ContextType { get; }
    }
}
