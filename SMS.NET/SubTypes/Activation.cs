using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SMS.NET.Helpers;
using SMS.NET.Models;
using SMS.NET.Models.Enums;
using SMS.NET.Models.Internal;
using SMS.NET.Models.Internal.Interfaces;

namespace SMS.NET.SubTypes
{
    public class Activation : IActivation
    {
        private SMS Sms { get; }

        internal Activation(SMS parent)
        {
            Sms = parent;
        }

        public async Task<List<ServiceQuantity>> GetAvailableNumbers(int? country = null, Operators operators = Operators.any)
        {
            List<ServiceQuantity> finalEnumerable = new();
            var responseMessage = await SMS.Client.CallAsync(Sms, "getNumbersStatus", new OArg<int?>("country", country).Fix(), new OArg<Operators>("operator", operators, false).Fix());
            var result = await responseMessage.Content.ReadAsStringAsync();
            foreach (var line in result.Replace(",", "").Replace("\"", "").Split("\n"))
                if (!line.Contains("{") && !line.Contains("}"))
                    finalEnumerable.Add(new ServiceQuantity(line));
            return finalEnumerable;
        }

        public async Task<Dictionary<string, TopServiceCountry>> GetTopCountries(string service = null, bool? freePrice = null)
        {
            var finalEnumerable = new Dictionary<string, TopServiceCountry>();
            var responseMessage = await SMS.Client.CallAsync(Sms, "getTopCountriesByService", new OArg<string>("service", service).Fix(), new OArg<bool?>("freePrice", freePrice).Fix());
            var resultStream = await responseMessage.Content.ReadAsStreamAsync();

            static async Task<TopCountryData> ParseData(string value)
            {
                await using MemoryStream nestedMs = await Converters.GetStreamAsync(value);
                var data = new TopCountryData();
                foreach (var nested in await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(nestedMs, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }))
                {
                    await using MemoryStream childMs = await Converters.GetStreamAsync(nested.Value.ToString());
                    switch (nested.Key)
                    {
                        case "country":
                            data.Country = int.Parse(nested.Value.ToString());
                            break;
                        case "count":
                            data.Count = int.Parse(nested.Value.ToString());
                            break;
                        case "price":
                            data.Price = double.Parse(nested.Value.ToString());
                            break;
                        case "freePriceMap":
                            data.FreePriceMap = await JsonSerializer.DeserializeAsync<Dictionary<double, int>>(childMs);
                            break;
                        case "retail_price":
                            data.RetailPrice = double.Parse(nested.Value.ToString());
                            break;
                    }
                }
                return data;
            }

            if (string.IsNullOrWhiteSpace(service))
            {
                foreach (var x in await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(resultStream))
                {
                    var tsc = new TopServiceCountry(x.Key);
                    await using MemoryStream ms = await Converters.GetStreamAsync(x.Value.ToString());
                    foreach (var (key, value) in await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(ms))
                    {
                        //This is not retail_price
                        if (int.TryParse(key, out int country))
                            tsc.TopCountries.Insert(country, await ParseData(value.ToString()));
                        else tsc.RetailPrice = double.Parse(value.ToString());
                    }

                    finalEnumerable.Add(x.Key, tsc);
                }
            }
            else
            {
                var tsc = new TopServiceCountry(service);
                foreach (var x in await JsonSerializer.DeserializeAsync<List<object>>(resultStream))
                    tsc.TopCountries.Add(await ParseData(x.ToString()));
                finalEnumerable.Add(service, tsc);
            }

            return finalEnumerable;
        }

        public async Task<Number> GetNumber(string service, int country, bool? forward = null, bool? freePrice = null,
            double? maxPrice = null, string phoneException = null, Operators operators = Operators.any,
            bool? verification = null, string referralId = null)
        {
            var responseMessage = await SMS.Client.CallAsync(Sms, "getNumber",
                new OArg<string>("service", service, false).Fix(), new OArg<int>("country", country, false).Fix(), new OArg<Operators>("operator", operators).Fix(), new OArg<bool?>("freePrice", freePrice).Fix(), new OArg<double?>("maxPrice", maxPrice).Fix(),
                new OArg<string>("phoneException", phoneException).Fix(), new OArg<bool?>("verification", verification).Fix(), new OArg<string>("ref", referralId).Fix(), new OArg<bool?>("forward", forward).Fix());
            var result = (await responseMessage.Content.ReadAsStringAsync()).Split(":");
            return new Number(long.Parse(result[1]), long.Parse(result[2]), Sms, DateTime.UtcNow);
        }

        [Obsolete("This endpoint does not respond properly. I have attempted to contact sms-activate.org to notify them about this issue but received no response from their team.", true)]
        public async Task<List<Number>> GetMultiServiceNumber(string[] services, int country, int[]? forward = null,
            Operators operators = Operators.any, string referralId = null)
        {
            var responseMessage = await SMS.Client.CallAsync(Sms, "getMultiServiceNumber",
                new OArg<string>("multiService", string.Join(",", services), false).Fix(), new OArg<int>("country", country, false).Fix(), new OArg<Operators>("operator", operators).Fix(), new OArg<string>("ref", referralId).Fix(), new OArg<string?>("multiForward", forward is null ? null : string.Join(",", forward)).Fix());
            var result = (await responseMessage.Content.ReadAsStringAsync()).Split(":");
            return null; //ENDPOINT NEVER RESPONDS
        }

        public async Task<Dictionary<int, CountryPrice>> GetPriceByCountry(string service = null, int? country = null)
        {
            Dictionary<int, CountryPrice> countryPrices = new();
            var responseMessage = await SMS.Client.CallAsync(Sms, "getPrices", new OArg<int?>("country", country).Fix(), new OArg<string>("service", service).Fix());
            var resultStream = await Converters.GetStreamAsync(await responseMessage.Content.ReadAsStringAsync());
            foreach (var x in await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(resultStream))
            {
                var countryId = int.Parse(x.Key);
                var countryPrice = new CountryPrice(countryId);
                var nestedStream = await Converters.GetStreamAsync(x.Value.ToString());
                foreach (var y in await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(nestedStream))
                {
                    var countryPriceData = new CountryPriceData(y.Key);
                    var childStream = await Converters.GetStreamAsync(y.Value.ToString());
                    foreach (var (key, value) in await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(childStream))
                    {
                        switch (key)
                        {
                            case "cost":
                                countryPriceData.Cost = double.Parse(value.ToString());
                                break;
                            case "count":
                                countryPriceData.Count = int.Parse(value.ToString());
                                break;
                        }
                    }
                    countryPrice.PriceData.Add(y.Key, countryPriceData);
                }
                
                countryPrices.Add(countryId, countryPrice);
            }
            return countryPrices;
        }

        public async Task<Dictionary<int, Country>> GetCountries()
        {
            Dictionary<int, Country> countries = new();
            var responseMessage = await SMS.Client.CallAsync(Sms, "getCountries");
            var resultStream = await Converters.GetStreamAsync(await responseMessage.Content.ReadAsStringAsync());
            foreach (var x in await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(resultStream))
            {
                var country = new Country();
                var nestedStream = await Converters.GetStreamAsync(x.Value.ToString());
                foreach (var (key, value) in await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(nestedStream))
                {
                    switch (key)
                    {
                        case "id":
                            country.Id = int.Parse(value.ToString());
                            break;
                        case "rus":
                            country.Russian = value.ToString();
                            break;
                        case "eng":
                            country.English = value.ToString();
                            break;
                        case "chn":
                            country.Chinese = value.ToString();
                            break;
                        case "visible":
                            country.Visible = Convert.ToBoolean(int.Parse(value.ToString()));
                            break;
                        case "retry":
                            country.Retry = Convert.ToBoolean(int.Parse(value.ToString()));
                            break;
                        case "rent":
                            country.Rent = Convert.ToBoolean(int.Parse(value.ToString()));
                            break;
                        case "multiService":
                            country.MultiService = Convert.ToBoolean(int.Parse(value.ToString()));
                            break;
                    }
                }
                countries.Add(int.Parse(x.Key), country);
            }
            return countries;
        }
    }
}
