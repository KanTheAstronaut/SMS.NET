using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SMS.NET.Models.Internal
{
    internal class IncomingCallStatusResponse
    {
        public string Status { get; set; }
        
        public bool Phone { get; set; }
    }
}
