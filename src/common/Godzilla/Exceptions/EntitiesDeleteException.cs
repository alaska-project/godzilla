using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class EntitiesDeleteException : GodzillaException
    {
        internal EntitiesDeleteException()
        {
        }

        internal EntitiesDeleteException(string message) : base(message)
        {
        }

        internal EntitiesDeleteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
