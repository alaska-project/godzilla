using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class NodeNotFoundException : GodzillaException
    {
        internal NodeNotFoundException()
        {
        }

        internal NodeNotFoundException(string message) : base(message)
        {
        }

        internal NodeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
