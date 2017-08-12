using System;
using System.Reflection.Emit;
#if COREFX
using System.Reflection;
#endif

namespace Nelibur.ObjectMapper.Core
{
    internal static class Helpers
    {
        internal static bool IsValueType(Type type)
        {
#if COREFX
            return type.GetTypeInfo().IsValueType;
#else
            return type.IsValueType;
#endif
        }

        internal static bool IsPrimitive(Type type)
        {
#if COREFX
            return type.GetTypeInfo().IsPrimitive;
#else
            return type.IsPrimitive;
#endif
        }

        internal static bool IsEnum(Type type)
        {
#if COREFX
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }

        internal static bool IsGenericType(Type type)
        {
#if COREFX
            return type.GetTypeInfo().IsGenericType;
#else
            return type.IsGenericType;
#endif
        }

        internal static Type CreateType(TypeBuilder typeBuilder)
        {
#if COREFX
            return typeBuilder.CreateTypeInfo().AsType();
#else
            return typeBuilder.CreateType();
#endif
        }

        internal static Type BaseType(Type type)
        {
#if COREFX
            return type.GetTypeInfo().BaseType;
#else
                return type.BaseType;
#endif
        }

    }
}