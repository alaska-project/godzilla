using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class UnauthorizedOperationException : GodzillaException
    {
        internal UnauthorizedOperationException()
        {
        }

        internal UnauthorizedOperationException(string message) : base(message)
        {
        }

        internal UnauthorizedOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
