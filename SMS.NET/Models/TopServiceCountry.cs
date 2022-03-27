using System;
using System.Collections.Generic;
using SMS.NET.SubTypes;

namespace SMS.NET.Models
{
    public class TopServiceCountry
    {
        public string Service { get; }
        /// <summary>
        /// This is going to be 0 (default) if the service IS specified in <see cref="Activation.GetTopCountries"/> you will need to use the one exposed by <see cref="TopCountryData"/>
        /// </summary>
        public double RetailPrice { get; internal set; }

        public List<TopCountryData> TopCountries { get; internal set; } = new();
        internal TopServiceCountry(string service)
        {
            Service = service;
        }
    }
}
