using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper.Core.DataStructures;

namespace Nelibur.ObjectMapper.Bindings
{
    internal class BindingConfig
    {
        private readonly HashSet<string> _ignoreFields;

        public BindingConfig(TypePair typePair)
        {
            TypePair = typePair;
            _ignoreFields = new HashSet<string>();
        }

        public TypePair TypePair { get; private set; }

        public bool IsIgnoreField(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return true;
            }
            return _ignoreFields.Contains(name);
        }

        protected void IgnoreField(string name)
        {
            _ignoreFields.Add(name);
        }
    }
}
