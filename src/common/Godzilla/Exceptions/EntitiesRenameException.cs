using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class EntitiesRenameException : GodzillaException
    {
        internal EntitiesRenameException()
        {
        }

        internal EntitiesRenameException(string message) : base(message)
        {
        }

        internal EntitiesRenameException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
