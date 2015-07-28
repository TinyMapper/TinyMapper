using System;

namespace Nelibur.ObjectMapper.Bindings
{
    public sealed class IgnoreAttribute : Attribute
    {
        public IgnoreAttribute()
        {
        }
    }

    public sealed class BindAttribute : Attribute
    {
        private readonly string _name;

        public BindAttribute(string name)
        {
            _name = name;
        }

        public string Name { get { return _name; } }
    }
}
