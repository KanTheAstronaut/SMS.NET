using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Models
{
    public class CountryPriceData
    {
        public string Service { get; }
        /// <summary>
        /// Price in rubles
        /// </summary>
        public double Cost { get; internal set; }
        /// <summary>
        /// Quantity left in stock
        /// </summary>
        public int Count { get; internal set; }
        internal CountryPriceData(string service)
        {
            Service = service;
        }
    }
}
