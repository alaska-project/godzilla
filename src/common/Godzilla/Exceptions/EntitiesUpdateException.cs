using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class EntitiesUpdateException : GodzillaException
    {
        internal EntitiesUpdateException()
        {
        }

        internal EntitiesUpdateException(string message) : base(message)
        {
        }

        internal EntitiesUpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
