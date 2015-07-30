using System;

namespace Nelibur.ObjectMapper.Bindings
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class IgnoreAttribute : Attribute
    {
        public IgnoreAttribute(Type bindToType = null)
        {
            BindToType = bindToType;
        }

        public Type BindToType { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class BindAttribute : Attribute
    {
        public BindAttribute(string name, Type bindToType = null)
        {
            Name = name;
            BindToType = bindToType;
        }

        public Type BindToType { get; private set; }
        public string Name { get; private set; }
    }
}
