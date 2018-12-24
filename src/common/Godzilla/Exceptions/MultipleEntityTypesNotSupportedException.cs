using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class MultipleEntityTypesNotSupportedException : GodzillaException
    {
        internal MultipleEntityTypesNotSupportedException()
        {
        }

        internal MultipleEntityTypesNotSupportedException(string message) : base(message)
        {
        }

        internal MultipleEntityTypesNotSupportedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
