using System;
using System.Collections.Generic;
using System.Text;
using SMS.NET.Models.Internal.Interfaces;

namespace SMS.NET.SubTypes
{
    public class Rent : IRent
    {
        private SMS Sms { get; }

        internal Rent(SMS parent)
        {
            Sms = parent;
        }
    }
}
