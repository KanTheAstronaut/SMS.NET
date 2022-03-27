using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Exceptions
{
    public class InvalidApiKey : Exception
    {
        internal InvalidApiKey()
        { }

        internal InvalidApiKey(string message) : base(message)
        { }

        internal InvalidApiKey(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
