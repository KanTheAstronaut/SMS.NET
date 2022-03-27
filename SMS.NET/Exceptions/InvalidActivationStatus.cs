using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Exceptions
{
    public class InvalidActivationStatus : Exception
    {
        internal InvalidActivationStatus()
        { }

        internal InvalidActivationStatus(string message) : base(message)
        { }

        internal InvalidActivationStatus(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
