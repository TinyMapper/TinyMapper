using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private static Type GetCollectionItemType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }
            else if (IsList(type))
            {
                return type.GetGenericArguments().First();
            }
            throw new NotSupportedException();
        }

        private static MethodInfo GetConverterImpl(TypePair typePair)
        {
            if (IsList(typePair.Target))
            {
                return typeof(CollectionTypeConverter).GetMethod("ConvertToList", BindingFlags.Static | BindingFlags.Public)
                                                      .MakeGenericMethod(GetCollectionItemType(typePair.Target));
            }
            throw new NotSupportedException();
        }

        private static bool IsList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}
