using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nelibur.ObjectMapper.Core.Extensions
{
    internal static class TypeExtensions
    {
        public static ConstructorInfo GetDefaultCtor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes);
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

        public static bool IsIEnumerable(this Type type)
        {
            return Types.IEnumerable.IsAssignableFrom(type);
        }

        public static bool IsIEnumerableOf(this Type type)
        {
            return type.GetInterfaces()
                       .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == Types.IEnumerableOf);
        }

        public static bool IsListOf(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
