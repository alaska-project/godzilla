using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class DuplicateCollectionIdException : GodzillaException
    {
        internal DuplicateCollectionIdException()
        {
        }

        internal DuplicateCollectionIdException(string message) : base(message)
        {
        }

        internal DuplicateCollectionIdException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
