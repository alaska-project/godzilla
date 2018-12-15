using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class EntityCreationException : GodzillaException
    {
        internal EntityCreationException()
        {
        }

        internal EntityCreationException(string message) : base(message)
        {
        }

        internal EntityCreationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
