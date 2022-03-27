using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Models
{
    public class ServiceQuantity
    {
        public string Service { get; }
        public bool Redirection { get; }
        public int Quantity { get; }
        internal ServiceQuantity(string line)
        {
            var fullLine = line.Split(":");
            var namePortion = fullLine[0].Split("_");
            Service = namePortion[0].Trim();
            Redirection = Convert.ToBoolean(int.Parse(namePortion[1]));
            Quantity = int.Parse(fullLine[1]);
        }
    }
}
