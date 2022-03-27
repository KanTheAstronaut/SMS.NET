using System;
using System.Collections.Generic;
using System.Text;
using SMS.NET.Helpers;

namespace SMS.NET.Models.Internal
{
    internal class OArg<T>
    {
        public string Key { get; }

        public T? Value { get; }

        private bool Nullable { get; }

        internal OArg(string key, T? value, bool nullable = true)
        {
            Key = key;
            Value = value;
            Nullable = nullable;
            if (!Nullable && Value is null)
                throw new ArgumentNullException(nameof(value), "Value can't be null in this scenario!");
        }

        public override string ToString() => Value is null && Nullable ? "" : $"{Key}={Value.ToLString()}";

        public bool Equals(OArg<T> obj) => !string.IsNullOrWhiteSpace(obj?.Key) && string.Equals(Key, obj.Key);

        public OArg<object> Fix() => new (Key, Value, Nullable);
    }
}
