using System;
using System.Net.Http;
using System.Threading.Tasks;
using SMS.NET.Exceptions;
using SMS.NET.Helpers;
using SMS.NET.Models.Internal.Interfaces;
using SMS.NET.SubTypes;

namespace SMS.NET
{
    public class SMS
    {
        internal static readonly SMSClient Client = new() {BaseAddress = new Uri("https://api.sms-activate.org/stubs/handler_api.php") };
        internal string _apiKey;

        #region Subtypes
        
        public Account Account { get; }
        public Activation Activation { get; }
        public Rent Rent { get; }

        #endregion

        /// <summary>
        /// This is the primary class used to connect to sms-activate.org's API using a specific API key.
        /// It is recommended to create one instance of this class and reusing it throughout your code UNLESS your API key is not static.
        /// </summary>
        /// <param name="apiKey">Your account's API key you can get that by going <a href="https://sms-activate.org/en/api2">here</a> (<c>https://sms-activate.org/en/api2</c>)</param>
        /// <param name="skipValidation">If false the code automatically attempts to run <see cref="IAccount.GetBalance"/> to verify that the API key is valid.</param>
        /// <exception cref="InvalidApiKey"></exception>
        public SMS(string apiKey, bool skipValidation = false)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidApiKey("API key can't be empty!");
            _apiKey = apiKey;
            
            Account = new Account(this);
            Activation = new Activation(this);
            Rent = new Rent(this);

            if (!skipValidation)
                Account.GetBalance().GetAwaiter().GetResult();
        }

    }
}
