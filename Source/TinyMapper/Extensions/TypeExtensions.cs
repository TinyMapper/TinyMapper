using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TinyMapper.Extensions
{
    internal static class TypeExtensions
    {
        public static ConstructorInfo GetDefaultCtor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes);
        }

        public static List<MemberInfo> GetPublicMembers(this Type type)
        {
            return type.GetMembers(BindingFlags.Instance | BindingFlags.Public)
                       .Where(x => x.MemberType == MemberTypes.Property || x.MemberType == MemberTypes.Field)
                       .ToList();
        }

        public static bool HasDefaultCtor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
