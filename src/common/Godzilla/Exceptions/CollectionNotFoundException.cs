using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class CollectionNotFoundException : GodzillaException
    {
        internal CollectionNotFoundException()
        {
        }

        internal CollectionNotFoundException(string message) : base(message)
        {
        }

        internal CollectionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
