using System;
using System.Collections.Generic;

namespace Nelibur.ObjectMapper.Bindings
{
    internal class BindingConfig
    {
        private readonly HashSet<string> _ignoreFields = new HashSet<string>();

        public bool IsIgnoreField(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return true;
            }
            return _ignoreFields.Contains(name);
        }

        internal void IgnoreField(string name)
        {
            _ignoreFields.Add(name);
        }
    }
}
