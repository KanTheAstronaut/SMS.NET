using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Exceptions
{
    public class InvalidOperator : Exception
    {
        internal InvalidOperator()
        { }

        internal InvalidOperator(string message) : base(message)
        { }

        internal InvalidOperator(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
