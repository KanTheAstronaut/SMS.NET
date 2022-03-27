using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SMS.NET.Exceptions;

namespace SMS.NET.Helpers
{
    internal static class ContextExtensions
    {
        public static async Task EnsureValidKey(this HttpResponseMessage message)
        {
            message.EnsureSuccessStatusCode();

            string responseMessage = await message.Content.ReadAsStringAsync();

            switch (responseMessage.ToLower())
            {
                case "bad_action":
                    throw new Exception("API HAS BEEN UPDATED PLEASE UPDATE SMS.NET TO THE LATEST VERSION OR WAIT FOR A NEW UPDATE!");
                case "bad_key":
                    throw new InvalidApiKey("API key is not valid!");
                case "no_key":
                    throw new InvalidApiKey("API key can't be empty!");
                case "wrong_operator":
                    throw new InvalidOperator("Invalid SMS operator!");
                case "sql_error":
                case "error_sql":
                    throw new SQLError("An SQL error has occurred on the server! Contact sms-activate.org!");
                case "bad_service":
                    throw new InvalidService("Invalid service name!");
                case "no_balance":
                    throw new NoBalance("Out of balance!");
                case "no_numbers":
                    throw new NoNumbers("Out of numbers for that country or service!");
                case "no_balance_forward":
                    throw new NoBalance("Balance is not enough to cover forwarding charges!");
                case "no_activation":
                case "invalid_activation_id":
                case "wrong_activation_id":
                    throw new InvalidActivationId("Invalid activation id!");
                case "bad_status":
                    throw new InvalidActivationStatus("Invalid activation status!");
                case "wrong_security":
                    throw new InvalidSecurity("An error occurred while trying to transfer an activation ID without forwarding, or a completed / inactive activation!");
                case "repeat_additional_service":
                    throw new RepeatedService("You can't order the same service again!");
                case "wrong_additional_service":
                    throw new InvalidService("Invalid additional service!");
                case string a when a.Contains("banned"):
                    throw new Banned($"You have been banned! Time: {a.Split(":")[1]}");
            }
        }
    }
}
