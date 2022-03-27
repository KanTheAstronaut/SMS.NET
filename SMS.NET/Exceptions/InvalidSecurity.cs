using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Exceptions
{
    public class InvalidSecurity : Exception
    {
        internal InvalidSecurity()
        { }

        internal InvalidSecurity(string message) : base(message)
        { }

        internal InvalidSecurity(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
