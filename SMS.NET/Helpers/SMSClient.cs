using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SMS.NET.Models.Internal;

namespace SMS.NET.Helpers
{
    internal class SMSClient : HttpClient
    {
        public async Task<HttpResponseMessage> CallAsync(SMS sms, string action, params OArg<object>[] additionalParams) => await CallAsync(sms._apiKey, action, additionalParams);
        public async Task<HttpResponseMessage> CallAsync(string apiKey, string action, params OArg<object>[] additionalParams) => await CallAsync($"?api_key={apiKey}&action={action}{ParseAdditionalParams(additionalParams)}");

        protected internal string ParseAdditionalParams(params OArg<object>[] additionalParams)
        {
            string final = string.Empty;
            foreach (var param in additionalParams)
            {
                if (param?.Value is null)
                    continue;
                final += $"&{param.Key}={param.Value.ToLString()}";
            }
            return final;
        }
        internal async Task<HttpResponseMessage> CallAsync(string requestUri)
        {
            var responseMessage = await GetAsync(requestUri);
            await responseMessage.EnsureValidKey();
            return responseMessage;
        }
    }
}
