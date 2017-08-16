using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;

namespace Nelibur.ObjectMapper.Bindings
{
    internal class BindingConfig
    {
        private readonly Dictionary<string, string> _bindFields = new Dictionary<string, string>();
        private readonly Dictionary<string, BindingFieldPath> _bindFieldsSourcePath = new Dictionary<string, BindingFieldPath>();
        private readonly Dictionary<string, BindingFieldPath> _bindFieldsTargetPath = new Dictionary<string, BindingFieldPath>();
        private readonly Dictionary<string, Type> _bindTypes = new Dictionary<string, Type>();
        private readonly Dictionary<string, Func<object, object>> _customTypeConverters = new Dictionary<string, Func<object, object>>();
        private readonly HashSet<string> _ignoreFields = new HashSet<string>();

        internal void BindConverter(string targetName, Func<object, object> func)
        {
            _customTypeConverters[targetName] = func;
        }

        internal void BindFields(List<string> sourcePath, List<string> targetPath)
        {
            var sourceFieldPath = new BindingFieldPath(sourcePath);
            var targetFieldPath = new BindingFieldPath(targetPath);

            _bindFields[sourceFieldPath.Head] = targetFieldPath.Head;
            _bindFieldsSourcePath[sourceFieldPath.Head] = sourceFieldPath;
            _bindFieldsTargetPath[targetFieldPath.Head] = targetFieldPath;
        }

        internal void BindType(string targetName, Type value)
        {
            _bindTypes[targetName] = value;
        }

        internal Option<string> GetBindField(string sourceName)
        {
            string result;
            bool exsist = _bindFields.TryGetValue(sourceName, out result);
            return new Option<string>(result, exsist);
        }

        internal Option<BindingFieldPath> GetBindFieldSourcePath(string fieldName)
        {
            BindingFieldPath result;
            bool exsist = _bindFieldsSourcePath.TryGetValue(fieldName, out result);
            return new Option<BindingFieldPath>(result, exsist);
        }

        internal Option<BindingFieldPath> GetBindFieldTargetPath(string fieldName)
        {
            BindingFieldPath result;
            bool exsist = _bindFieldsTargetPath.TryGetValue(fieldName, out result);
            return new Option<BindingFieldPath>(result, exsist);
        }

        internal Option<Type> GetBindType(string targetName)
        {
            Type result;
            bool exsist = _bindTypes.TryGetValue(targetName, out result);
            return new Option<Type>(result, exsist);
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
