using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class EntitiesCreationException : GodzillaException
    {
        internal EntitiesCreationException()
        {
        }

        internal EntitiesCreationException(string message) : base(message)
        {
        }

        internal EntitiesCreationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
