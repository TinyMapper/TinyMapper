using System;

namespace Nelibur.ObjectMapper.Bindings
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class IgnoreAttribute : Attribute
    {
        private readonly Type _bindToType;

        public IgnoreAttribute(Type bindToType = null)
        {
            _bindToType = bindToType;
        }

        public Type BindToType { get { return _bindToType; } }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class BindAttribute : Attribute
    {
        private readonly string _name;
        private readonly Type _bindToType;

        public BindAttribute(string name, Type bindToType = null)
        {
            _name = name;
            _bindToType = bindToType;
        }

        public string Name { get { return _name; } }
        public Type BindToType { get { return _bindToType; } }
    }
}
