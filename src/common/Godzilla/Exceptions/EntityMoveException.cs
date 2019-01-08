using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class EntityMoveException : GodzillaException
    {
        internal EntityMoveException()
        {
        }

        internal EntityMoveException(string message) : base(message)
        {
        }

        internal EntityMoveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
