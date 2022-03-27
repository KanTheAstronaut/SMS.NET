using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Exceptions
{
    public class NoNumbers : Exception
    {
        internal NoNumbers()
        { }

        internal NoNumbers(string message) : base(message)
        { }

        internal NoNumbers(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
