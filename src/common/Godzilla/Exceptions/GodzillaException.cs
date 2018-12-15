using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Godzilla.Exceptions
{
    public class GodzillaException : Exception
    {
        internal GodzillaException()
        {
        }

        internal GodzillaException(string message) : base(message)
        {
        }

        internal GodzillaException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
