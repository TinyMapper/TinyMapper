using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;

namespace Nelibur.ObjectMapper.Bindings
{
    internal class BindingConfig
    {
        private readonly Dictionary<string, List<string>> _oneToOneBindFields = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, List<BindingFieldPath>> _bindFieldsPath = new Dictionary<string, List<BindingFieldPath>>();
        private readonly Dictionary<string, Type> _bindTypes = new Dictionary<string, Type>();
        private readonly Dictionary<string, Func<object, object>> _customTypeConverters = new Dictionary<string, Func<object, object>>();
        private readonly HashSet<string> _ignoreFields = new HashSet<string>();

        internal void BindConverter(string targetName, Func<object, object> func)
        {
            _customTypeConverters[targetName] = func;
        }

        internal void BindFields(List<string> sourcePath, List<string> targetPath)
        {
            var bindingFieldPath = new BindingFieldPath(sourcePath, targetPath);

            if (!bindingFieldPath.HasPath)
            {
                if (_oneToOneBindFields.ContainsKey(bindingFieldPath.SourceHead))
                {
                    _oneToOneBindFields[bindingFieldPath.SourceHead].Add(bindingFieldPath.TargetHead);
                }
                else
                {
                    _oneToOneBindFields[bindingFieldPath.SourceHead] = new List<string>{ bindingFieldPath.TargetHead };
                }
            }
            else
            {
                if (_bindFieldsPath.ContainsKey(bindingFieldPath.SourceHead))
                {
                    _bindFieldsPath[bindingFieldPath.SourceHead].Add(bindingFieldPath);
                }
                else
                {
                    _bindFieldsPath[bindingFieldPath.SourceHead] = new List<BindingFieldPath> { bindingFieldPath };
                }
            }
        }

        internal void BindType(string targetName, Type value)
        {
            _bindTypes[targetName] = value;
        }

        internal Option<List<string>> GetBindField(string sourceName)
        {
            List<string> result;
            bool exists = _oneToOneBindFields.TryGetValue(sourceName, out result);
            return new Option<List<string>> (result, exists);
        }

        internal Option<List<BindingFieldPath>> GetBindFieldPath(string fieldName)
        {
            List<BindingFieldPath> result;
            bool exists = _bindFieldsPath.TryGetValue(fieldName, out result);
            return new Option<List<BindingFieldPath>>(result, exists);
        }

        internal Option<Type> GetBindType(string targetName)
        {
            Type result;
            bool exists = _bindTypes.TryGetValue(targetName, out result);
            return new Option<Type>(result, exists);
        }

        internal Option<Func<object, object>> GetCustomTypeConverter(string targetName)
        {
            return _customTypeConverters.GetValue(targetName);
        }

        internal bool HasCustomTypeConverter(string targetName)
        {
            return _customTypeConverters.ContainsKey(targetName);
        }

        internal void IgnoreSourceField(string sourceName)
        {
            _ignoreFields.Add(sourceName);
        }

        internal bool IsIgnoreSourceField(string sourceName)
        {
            if (string.IsNullOrEmpty(sourceName))
            {
                return true;
            }
            return _ignoreFields.Contains(sourceName);
        }
    }
}
