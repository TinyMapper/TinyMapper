using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper.Core.DataStructures;

namespace Nelibur.ObjectMapper.Bindings
{
    internal class BindingConfig
    {
        private readonly Dictionary<string, string> _bindFields = new Dictionary<string, string>();
        private readonly HashSet<string> _ignoreFields = new HashSet<string>();

        public Option<string> GetBindField(string sourceName)
        {
            string result;
            bool exsist = _bindFields.TryGetValue(sourceName, out result);
            return new Option<string>(result, exsist);
        }

        public bool IsIgnoreField(string sourceName)
        {
            if (string.IsNullOrEmpty(sourceName))
            {
                return true;
            }
            return _ignoreFields.Contains(sourceName);
        }

        internal void BindFields(string sourceName, string targetName)
        {
            _bindFields[sourceName] = targetName;
        }

        internal void IgnoreField(string sourceName)
        {
            _ignoreFields.Add(sourceName);
        }
    }
}
