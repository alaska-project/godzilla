using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Exceptions
{
    public class ParentNodeNotFoundException : GodzillaException
    {
        internal ParentNodeNotFoundException()
        {
        }

        internal ParentNodeNotFoundException(string message) : base(message)
        {
        }

        internal ParentNodeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
