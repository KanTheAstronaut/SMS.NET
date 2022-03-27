using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Exceptions
{
    public class InvalidException : Exception
    {
        internal InvalidException()
        { }

        internal InvalidException(string message) : base(message)
        { }

        internal InvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
