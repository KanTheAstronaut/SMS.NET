using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Exceptions
{
    public class Banned : Exception
    {
        internal Banned()
        { }

        internal Banned(string message) : base(message)
        { }

        internal Banned(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
