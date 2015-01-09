using System;

namespace TinyMapper.Engines.Configs
{
    internal sealed class MapConfig
    {
        public bool Match(string valueA, string valueB)
        {
            return string.Equals(valueA, valueB, StringComparison.Ordinal);
        }
    }
}
