using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SMS.NET.Models
{
    public class Country
    {
        public int Id { get; internal set; }
        public string Russian { get; internal set; }
        public string English { get; internal set; }
        public string Chinese { get; internal set; }
        public bool Visible { get; internal set; }
        public bool Retry { get; internal set; }
        public bool Rent { get; internal set; }
        public bool MultiService { get; internal set; }

        internal Country()
        {

        }
    }
}
