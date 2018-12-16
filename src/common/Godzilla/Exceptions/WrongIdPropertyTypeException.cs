using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class WrongIdPropertyTypeException : GodzillaException
    {
        internal WrongIdPropertyTypeException()
        {
        }

        internal WrongIdPropertyTypeException(string message) : base(message)
        {
        }

        internal WrongIdPropertyTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
