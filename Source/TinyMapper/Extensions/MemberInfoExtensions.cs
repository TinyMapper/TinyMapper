using System;
using System.Reflection;
using TinyMapper.Nelibur.Sword.Patterns;

namespace TinyMapper.Extensions
{
    internal static class MemberInfoExtensions
    {
        private static readonly IFuncVisitor<MemberInfo, Type> _visitor = Visitor.For<MemberInfo, Type>()
                                                                                 .Register<PropertyInfo>(x => x.PropertyType)
                                                                                 .Register<FieldInfo>(x => x.FieldType)
                                                                                 .Register<MethodInfo>(x => x.ReturnType);

        public static Type GetMemberType(this MemberInfo value)
        {
            return _visitor.Visit(value);
        }
    }
}
