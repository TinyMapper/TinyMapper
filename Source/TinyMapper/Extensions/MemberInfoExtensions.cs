using System;
using System.Reflection;

namespace TinyMappers.Extensions
{
    internal static class MemberInfoExtensions
    {
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

        public static bool IsMethod(this MemberInfo value)
        {
            return value.MemberType == MemberTypes.Method;
        }

        public static bool IsProperty(this MemberInfo value)
        {
            return value.MemberType == MemberTypes.Property;
        }
    }
}
