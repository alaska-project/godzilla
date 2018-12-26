using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class EntitiesNotFoundException : GodzillaException
    {
        internal EntitiesNotFoundException()
        {
        }

        internal EntitiesNotFoundException(string message) : base(message)
        {
        }

        internal EntitiesNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
