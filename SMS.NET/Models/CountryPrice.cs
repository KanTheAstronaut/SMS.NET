using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Models
{
    public class CountryPrice
    {
        /// <summary>
        /// Country id
        /// </summary>
        public int Country { get; }
        public Dictionary<string, CountryPriceData> PriceData { get; internal set; } = new();
        internal CountryPrice(int country)
        {
            Country = country;
        }
    }
}
