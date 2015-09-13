using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nelibur.ObjectMapper.Core.DataStructures;

namespace Nelibur.ObjectMapper.Core.Extensions
{
    internal static class MemberInfoExtensions
    {
        public static Option<TAttribute> GetAttribute<TAttribute>(this MemberInfo value)
            where TAttribute : Attribute
        {
            return value.GetCustomAttributes()
                        .FirstOrDefault(x => x is TAttribute)
                        .ToType<TAttribute>();
        }

        public static List<TAttribute> GetAttributes<TAttribute>(this MemberInfo value)
            where TAttribute : Attribute
        {
            return value.GetCustomAttributes().OfType<TAttribute>().ToList();
        }

        public static Type GetMemberType(this MemberInfo value)
        {
            if (value.IsField())
            {
                return ((FieldInfo)value).FieldType;
            }
            else if (value.IsProperty())
            {
                return ((PropertyInfo)value).PropertyType;
            }
            else if (value.IsMethod())
            {
                return ((MethodInfo)value).ReturnType;
            }
            throw new NotSupportedException();
        }

        public static bool IsField(this MemberInfo value)
        {
            return value.MemberType == MemberTypes.Field;
        }

        public static bool IsProperty(this MemberInfo value)
        {
            return value.MemberType == MemberTypes.Property;
        }

        private static bool IsMethod(this MemberInfo value)
        {
            return value.MemberType == MemberTypes.Method;
        }
    }
}
