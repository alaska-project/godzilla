using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class NodeAlreadyExistsException : GodzillaException
    {
        internal NodeAlreadyExistsException()
        {
        }

        internal NodeAlreadyExistsException(string message) : base(message)
        {
        }

        internal NodeAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
