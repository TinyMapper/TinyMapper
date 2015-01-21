using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TinyMapper.DataStructures;

namespace TinyMapper.TypeConverters
{
    internal sealed class CollectionTypeConverter
    {
        public static List<T> ConvertToList<T>(IEnumerable source)
        {
            var result = new List<T>();
            foreach (object item in source)
            {
                result.Add((T)item);
            }
            return result;
        }

        public static MethodInfo GetConverter(TypePair typePair)
        {
            MethodInfo result = GetConverterImpl(typePair);
            return result;
        }

        public static bool IsSupported(TypePair typePair)
        {
            return typePair.Target.IsArray
                   || typeof(IEnumerable).IsAssignableFrom(typePair.Target);
        }

        private static MethodInfo GetConverterImpl(TypePair typePair)
        {
            if (IsTargetTypeList(typePair))
            {
                return typeof(CollectionTypeConverter).GetMethod("ConvertToList", BindingFlags.Static | BindingFlags.Public)
                                                      .MakeGenericMethod(typePair.Target);
            }
            return null;
        }

        private static bool IsTargetTypeList(TypePair typePair)
        {
            return typePair.Target.IsGenericType && typePair.Target.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}
