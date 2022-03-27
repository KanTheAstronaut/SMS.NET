using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMS.NET.Models.Internal.Interfaces;

namespace SMS.NET.SubTypes
{
    public class Account : IAccount
    {
        private SMS Sms { get; }

        internal Account(SMS parent)
        {
            Sms = parent;
        }

        public async Task<double> GetBalance()
        {
            var responseMessage = await SMS.Client.CallAsync(Sms._apiKey, "getBalance");
            var result = await responseMessage.Content.ReadAsStringAsync();
            return double.Parse(result.Split(":")[1]);
        }
    }
}
