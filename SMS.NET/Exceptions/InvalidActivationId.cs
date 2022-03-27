using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Exceptions
{
    public class InvalidActivationId : Exception
    {
        internal InvalidActivationId()
        { }

        internal InvalidActivationId(string message) : base(message)
        { }

        internal InvalidActivationId(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
