using System;

namespace TinyMapper.Configs
{
    internal sealed class MapConfig
    {
        public bool Match(string valueA, string valueB)
        {
            return string.Equals(valueA, valueB, StringComparison.Ordinal);
        }
    }
}
