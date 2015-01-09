using System;

namespace TinyMapper.Builders
{
    internal static class MapperTypeNameBuilder
    {
        private const string Prefix = "TinyMapper";

        public static string Build(Type source, Type target)
        {
            return string.Format("{0}_{1}_{2}", Prefix, GetFullName(source), GetFullName(target));
        }

        private static string GetFullName(Type type)
        {
            return type == null ? "Empty" : type.FullName;
        }
    }
}
