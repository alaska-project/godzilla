using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class MissingIdException : GodzillaException
    {
        internal MissingIdException()
        {
        }

        internal MissingIdException(string message) : base(message)
        {
        }

        internal MissingIdException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
