using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Exceptions
{
    public class InvalidService : Exception
    {
        internal InvalidService()
        { }

        internal InvalidService(string message) : base(message)
        { }

        internal InvalidService(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
