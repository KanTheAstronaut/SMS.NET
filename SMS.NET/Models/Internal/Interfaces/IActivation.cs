using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMS.NET.Models.Enums;
using SMS.NET.Exceptions;

namespace SMS.NET.Models.Internal.Interfaces
{
    internal interface IActivation
    {
        /// <summary>
        /// Gets the amount of numbers left in stock for each service for a selected country
        /// </summary>
        /// <param name="country">The country to check the stock for. You can also cast <see cref="Countries"/> to an <see cref="int"/>
        /// <remarks>Also note that despite this parameter being optional sms-activate will set it to <see cref="Countries.Russia"/> (0) by default</remarks></param>
        /// <param name="operators">The operator to filter the results according to</param>
        /// <returns>A list of <see cref="ServiceQuantity"/> objects which contain the name of the service, whether redirection is enabled or not, and the quantity left in stock</returns>
        public Task<List<ServiceQuantity>> GetAvailableNumbers(int? country = null, Operators operators = Operators.any);
        
        /// <summary>
        /// Gets the top few countries by service
        /// </summary>
        /// <param name="service">The service that you want to filter your results by
        /// <remarks>You can find all services <a href="https://sms-activate.org/en/api2">here</a> (<c>https://sms-activate.org/en/api2</c>)</remarks></param>
        /// <param name="freePrice">If true, the quantity and price will be transferred according to the Free Price (from official docs)</param>
        /// <returns>A dictionary of the results where the <see cref="KeyValuePair{TKey,TValue}.Key"/> refers to the service (in code form)
        /// and the <see cref="KeyValuePair{TKey,TValue}.Value"></see> contains a <see cref="TopServiceCountry"/> object that in return contains country data (prices, quantity, etc...)</returns>
        public Task<Dictionary<string, TopServiceCountry>> GetTopCountries(string service = null, bool? freePrice = null);

        /// <summary>
        /// Requests a number from sms-activate.org with the specified paramaters
        /// </summary>
        /// <param name="service">The service that you want to receive your SMS from
        /// <remarks>You can find all services <a href="https://sms-activate.org/en/api2">here</a> (<c>https://sms-activate.org/en/api2</c>)</remarks></param>
        /// <param name="country">The number's country. You can also cast <see cref="Countries"/> to an <see cref="int"/></param>
        /// <param name="forward">Whether the number should support forwarding</param>
        /// <param name="freePrice">buy a number using Free Price.
        /// <remarks>optional parameter accepts  true(use Free Price). You also need to pass the parameter <param name="maxPrice"></param> (from official docs)</remarks></param>
        /// <param name="maxPrice">indicate the maximum price at which you are ready to buy a number at Free Price.
        /// <remarks>optional parameter, specified only if <param name="freePrice"></param> = true (from official docs)</remarks></param>
        /// <param name="phoneException">Exclude specific phone number prefixes
        /// <remarks>Accepts multiple entries separate your entries using commas like so (e.g "7918,7900111")</remarks></param>
        /// <param name="operators">The mobile operator of the number that you want to get</param>
        /// <param name="verification">If transmitted true, get a number with the ability to receive a call (from official docs)</param>
        /// <param name="referralId">Affiliate code which you can find <a href="https://sms-activate.org/en/pp">here</a> (<c>https://sms-activate.org/en/pp</c>)</param>
        /// <returns>A <see cref="Number"/> object with the activation id and the phone number
        /// <remarks>The <see cref="Number.CreatedAt"/> <see cref="DateTime"/> variable in the <see cref="Number"/> returned by this method is never null</remarks></returns>
        /// <exception cref="NoNumbers"></exception>
        /// <exception cref="NoBalance"></exception>
        /// <exception cref="InvalidService"></exception>
        /// <exception cref="SQLError"></exception>
        /// <exception cref="Banned"></exception>
        /// <exception cref="InvalidException"></exception>
        public Task<Number> GetNumber(string service, int country, bool? forward = null, bool? freePrice = null,
            double? maxPrice = null, string phoneException = null, Operators operators = Operators.any,
            bool? verification = null, string referralId = null);

        /// <summary>
        /// This method was initially meant for requesting a number for multiple services.
        /// However, after further inspection, the endpoint responsible for doing so is not working correctly and doesn't respond.
        /// I've tried to contact the technical support team but received no response.
        /// </summary>
        public Task<List<Number>> GetMultiServiceNumber(string[] services, int country, int[]? forward = null,
            Operators operators = Operators.any, string referralId = null);

        /// <summary>
        /// Gets the prices of ordering numbers for each country depending on the service
        /// </summary>
        /// <param name="service">The service that you want to filter your results by
        /// <remarks>You can find all services <a href="https://sms-activate.org/en/api2">here</a> (<c>https://sms-activate.org/en/api2</c>)</remarks></param>
        /// <param name="country">The country that you want to filter your results by
        /// <remarks>If left null it will default to all countries</remarks></param>
        /// <returns>A dictionary with the <see cref="KeyValuePair{TKey,TValue}.Key"/> set to the country id and the <see cref="KeyValuePair{TKey,TValue}.Value"/> containing price data by service</returns>
        public Task<Dictionary<int, CountryPrice>> GetPriceByCountry(string service = null, int? country = null);

        /// <summary>
        /// Gets all countries that sms-activate.org supports including ones that aren't visible or enabled
        /// </summary>
        /// <returns>A dictionary with the <see cref="KeyValuePair{TKey,TValue}.Key"/> set to the country id and the <see cref="KeyValuePair{TKey,TValue}.Value"/> containing full country data (name, visibility, etc...)</returns>
        public Task<Dictionary<int, Country>> GetCountries();
    }
}
