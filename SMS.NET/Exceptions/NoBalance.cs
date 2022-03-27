using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Exceptions
{
    public class NoBalance : Exception
    {
        internal NoBalance()
        { }

        internal NoBalance(string message) : base(message)
        { }

        internal NoBalance(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
