using System;
using System.Collections.Generic;
using SMS.NET.Models.Internal.Interfaces;

namespace SMS.NET.Models
{
    public class TopCountryData
    {
        public int Country { get; internal set; }
        public long Count { get; internal set; }
        public double Price { get; internal set; }
        /// <summary>
        /// This is going to be 0 (default) if the service is NOT specified in <see cref="IActivation.GetTopCountries"/> you will need to use the one exposed by <see cref="TopServiceCountry"/>
        /// </summary>
        public double RetailPrice { get; internal set; }
        /// <summary>
        /// Will always be empty if the freePrice parameter in <see cref="Activation.GetTopCountries"/> is left as null or set to false
        /// </summary>
        public Dictionary<double, int> FreePriceMap { get; internal set; } = new();

        internal TopCountryData()
        {

        }
    }
}
