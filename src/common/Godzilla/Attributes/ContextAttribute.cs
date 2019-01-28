using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Attributes
{
    public class ContextAttribute : Attribute
    {
        public ContextAttribute()
        { }

        public ContextAttribute(string contextId)
        {
            ContextId = contextId;
        }

        public string ContextId { get; }
    }
}
