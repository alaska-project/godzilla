using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class WrongCollectionElementTypeException : GodzillaException
    {
        internal WrongCollectionElementTypeException()
        {
        }

        internal WrongCollectionElementTypeException(string message) : base(message)
        {
        }

        internal WrongCollectionElementTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
