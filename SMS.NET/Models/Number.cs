using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SMS.NET.Helpers;
using SMS.NET.Models.Enums;
using SMS.NET.Exceptions;
using SMS.NET.Models.Internal;
using SMS.NET.Models.Internal.Interfaces;

namespace SMS.NET.Models
{
    public class Number
    {
        public long PhoneNumber { get; }
        public long Id { get; }
        /// <summary>
        /// returns null if the <see cref="Number"/> object was not created internally from <see cref="IActivation.GetNumber"/> or <see cref="IActivation.GetMultiServiceNumber"/> (obsolete)
        /// </summary>
        public DateTime? CreatedAt { get; }
        private SMS Sms { get; }
        internal Number(long id, long number, SMS sms, DateTime? time = null)
        {
            Id = id;
            PhoneNumber = number;
            CreatedAt = time;
            Sms = sms;
        }

        /// <summary>
        /// ONLY use this constructor if you want to control a custom activation id it is preferred to <see cref="Number"/> from <see cref="IActivation.GetNumber"/> as it internally sets everything properly
        /// <remarks>Note that if you use this constructor then <see cref="CreatedAt"/> will be null</remarks>
        /// </summary>
        /// <param name="id">The activation id</param>
        /// <param name="sms">The <see cref="SMS"/> object created with your preferred API key</param>
        public Number(long id, SMS sms)
        {
            Id = id;
            Sms = sms;
        }

        public override string ToString() => $"+{PhoneNumber}";

        /// <summary>
        /// Gets the current number's activation status
        /// </summary>
        /// <returns>A <see cref="KeyValuePair{TKey,TValue}"/> where the <see cref="KeyValuePair{TKey,TValue}.Key"/> is the status and the <see cref="KeyValuePair{TKey,TValue}.Value"/> is the SMS code if present</returns>
        /// <exception cref="InvalidActivationId"></exception>
        /// <exception cref="SQLError"></exception>
        public async Task<KeyValuePair<ActivationStatus, string>> GetActivationStatus()
        {
            var responseMessage = await SMS.Client.CallAsync(Sms, "getStatus", new OArg<long>("id", Id, false).Fix());
            var result = await responseMessage.Content.ReadAsStringAsync();
            if (result.Contains(":"))
            {
                var splitResult = result.Split(":");
                return new KeyValuePair<ActivationStatus, string>(Enum.Parse<ActivationStatus>(splitResult[0], true), splitResult.Length > 1 ? splitResult[1] : null);
            }
            return new KeyValuePair<ActivationStatus, string>(Enum.Parse<ActivationStatus>(result, true), null);
        }

        public async Task<ActivationStatus> GetStatus() => (await GetActivationStatus()).Key;
        public async Task<string> GetSms() => (await GetActivationStatus()).Value;

        /// <summary>
        /// Sets the current number's activation status
        /// </summary>
        /// <param name="status">The status that you want to set
        /// <remarks>You can cast <see cref="ActivationStatus"/> or <see cref="AccessStatus"/> to an <see cref="int"/> to use the as the status</remarks></param>
        /// <param name="forward">phone number to which you want to forward (from official docs)</param>
        /// <returns>A <see cref="AccessStatus"/> to confirm the status of the current number</returns>
        /// <exception cref="InvalidActivationId"></exception>
        /// <exception cref="InvalidService"></exception>
        /// <exception cref="InvalidActivationStatus"></exception>
        /// <exception cref="SQLError"></exception>
        public async Task<AccessStatus> ChangeActivationStatus(int status, string forward = null)
        {
            var responseMessage = await SMS.Client.CallAsync(Sms, "setStatus", new OArg<long>("id", Id, false).Fix(), new OArg<int>("status", status, false).Fix(), new OArg<string>("forward", forward).Fix());
            var result = await responseMessage.Content.ReadAsStringAsync();
            return Enum.Parse<AccessStatus>(result);
        }

        /// <summary>
        /// Gets the status of an incoming call from the current number
        /// </summary>
        /// <returns>A <see cref="KeyValuePair{TKey,TValue}"/> where <see cref="KeyValuePair{TKey,TValue}.Key"/> is the status and <see cref="KeyValuePair{TKey,TValue}.Value"/> is the <c>phone</c> variable</returns>
        /// <exception cref="InvalidActivationId"></exception>
        public async Task<KeyValuePair<IncomingCallStatus, bool>> GetIncomingCallStatus()
        {
            var responseMessage = await SMS.Client.CallAsync(Sms, "getIncomingCallStatus", new OArg<long>("id", Id, false).Fix());
            var result = await responseMessage.Content.ReadAsStringAsync();
            var parsedResult = await JsonSerializer.DeserializeAsync<IncomingCallStatusResponse>(await Converters.GetStreamAsync(result), new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
            return new KeyValuePair<IncomingCallStatus, bool>((IncomingCallStatus)int.Parse(parsedResult.Status), parsedResult.Phone);
        }

        /// <summary>
        /// <para>Having received the first SMS for redirected numbers, you can buy other related services such as Yula, Yandex, Avito, Any other</para>
        /// <para>The cost of additional service is only 5p (from official docs)</para>
        /// </summary>
        /// <param name="service">The service that you want to receive an SMS from
        /// <remarks>Note that <param name="service"></param> must not be the same service as the original service or else a <see cref="RepeatedService"/> exception will be thrown
        /// and it must be related to the original service or else a <see cref="InvalidService"/> exception will be thrown</remarks></param>
        /// <returns>A new <see cref="Number"/> object with the id of the new activation but with the same phone number</returns>
        /// <exception cref="InvalidService"></exception>
        /// <exception cref="InvalidActivationId"></exception>
        /// <exception cref="InvalidSecurity"></exception>
        /// <exception cref="RepeatedService"></exception>
        /// <exception cref="SQLError"></exception>
        public async Task<Number> GetAdditionalService(string service)
        {
            var responseMessage = await SMS.Client.CallAsync(Sms, "getAdditionalService", new OArg<long>("id", Id, false).Fix(), new OArg<string>("service", service, false).Fix());
            var result = await responseMessage.Content.ReadAsStringAsync();
            var splitResult = result.Split(":");
            return new Number(long.Parse(splitResult[1]), long.Parse(splitResult[2]), Sms, DateTime.UtcNow);
        }
    }
}
