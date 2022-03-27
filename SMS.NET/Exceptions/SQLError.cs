using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Exceptions
{
    public class SQLError : Exception
    {
        internal SQLError()
        { }

        internal SQLError(string message) : base(message)
        { }

        internal SQLError(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
