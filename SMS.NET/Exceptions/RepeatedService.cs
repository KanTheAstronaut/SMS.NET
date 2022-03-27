using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Exceptions
{
    public class RepeatedService : Exception
    {
        internal RepeatedService()
        { }

        internal RepeatedService(string message) : base(message)
        { }

        internal RepeatedService(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
