using System;

namespace Nelibur.ObjectMapper.Bindings
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class IgnoreAttribute : Attribute
    {
        public IgnoreAttribute(Type targetType = null)
        {
            TargetType = targetType;
        }

        public Type TargetType { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class BindAttribute : Attribute
    {
        public BindAttribute(string memberName, Type targetType = null)
        {
            MemberName = memberName;
            TargetType = targetType;
        }

        public Type TargetType { get; private set; }
        public string MemberName { get; private set; }
    }
}
