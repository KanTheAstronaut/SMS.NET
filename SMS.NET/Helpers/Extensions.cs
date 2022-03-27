using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.NET.Helpers
{
    internal static class Extensions
    {
        public static string ToLString<T>(this T value) => value.ToString().ToLower();
    }
}
