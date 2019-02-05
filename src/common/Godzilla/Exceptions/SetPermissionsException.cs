using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class SetPermissionsException : GodzillaException
    {
        internal SetPermissionsException()
        {
        }

        internal SetPermissionsException(string message) : base(message)
        {
        }

        internal SetPermissionsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
