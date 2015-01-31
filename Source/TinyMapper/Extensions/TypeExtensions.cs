using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TinyMappers.Extensions
{
    internal static class TypeExtensions
    {
        public static ConstructorInfo GetDefaultCtor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes);
        }

        public static bool HasDefaultCtor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsGenericInterfaceImplemented(this Type type, Type @interface)
        {
            return type.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == @interface);
        }
    }
}
