using System.Threading.Tasks;

namespace SMS.NET.Models.Internal.Interfaces
{
    internal interface IAccount
    {
        /// <summary>
        /// Gets the current user's balance
        /// </summary>
        /// <returns>The current user's balance as a <see cref="double"/></returns>
        public Task<double> GetBalance();
    }
}
