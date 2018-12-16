using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class MissingIdPropertyException : GodzillaException
    {
        internal MissingIdPropertyException()
        {
        }

        internal MissingIdPropertyException(string message) : base(message)
        {
        }

        internal MissingIdPropertyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
