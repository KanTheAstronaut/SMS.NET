using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SMS.NET.Helpers
{
    internal static class Converters
    {
        public static async Task<MemoryStream> GetStreamAsync(string value)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            await sw.WriteAsync(value);
            await sw.FlushAsync();
            ms.Position = 0;
            return ms;
        }
    }
}
