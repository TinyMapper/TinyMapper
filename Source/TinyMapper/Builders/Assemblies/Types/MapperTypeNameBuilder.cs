using System;
using TinyMapper.DataStructures;

namespace TinyMapper.Builders.Assemblies.Types
{
    internal static class MapperTypeNameBuilder
    {
        private const string Prefix = "TinyMapper";

        public static string Build(TypePair pair)
        {
            string random = Guid.NewGuid().ToString("N");
            return string.Format("{0}_{1}_{2}_{3}", Prefix, GetFullName(pair.Source), GetFullName(pair.Target), random);
        }

        private static string GetFullName(Type type)
        {
            return type == null ? "Empty" : type.FullName;
        }
    }
}
