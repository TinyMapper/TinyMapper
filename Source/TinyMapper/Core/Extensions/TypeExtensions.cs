using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nelibur.ObjectMapper.Core.Extensions
{
    internal static class TypeExtensions
    {
        public static Type GetCollectionItemType(this Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }
            else if (type.IsGenericType && type.IsIEnumerableOf())
            {
                return type.GetGenericArguments().First();
            }

            return typeof(object);
        }

        public static ConstructorInfo GetDefaultCtor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes);
        }

        public static KeyValuePair<Type, Type> GetDictionaryItemTypes(this Type type)
        {
            if (type.IsDictionaryOf())
            {
                Type[] types = type.GetGenericArguments();
                return new KeyValuePair<Type, Type>(types[0], types[1]);
            }
            throw new NotSupportedException();
        }

        public static MethodInfo GetGenericMethod(this Type type, string methodName, params Type[] arguments)
        {
            return type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic)
                       .MakeGenericMethod(arguments);
        }

        public static bool HasDefaultCtor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }

        public static bool IsDictionaryOf(this Type type)
        {
            return type.IsGenericType &&
                   (type.GetGenericTypeDefinition() == typeof(Dictionary<,>) ||
                    type.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        public static bool IsIEnumerable(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static bool IsIEnumerableOf(this Type type)
        {
            return type.GetInterfaces()
                       .Any(x => x.IsGenericType &&
                                 x.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                                 (!x.IsGenericType && x == typeof(IEnumerable)));
        }

        public static bool IsListOf(this Type type)
        {
            return
            type.IsGenericType &&
             (type.GetGenericTypeDefinition() == typeof(List<>) ||
              type.GetGenericTypeDefinition() == typeof(IList<>) ||
              type.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
