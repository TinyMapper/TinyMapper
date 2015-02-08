using System;
using System.Reflection;
using Nelibur.ObjectMapper.Core.DataStructures;

namespace Nelibur.ObjectMapper.Mappers
{
    internal abstract class MapperBuilder
    {
        protected const MethodAttributes OverrideProtected = MethodAttributes.Family | MethodAttributes.Virtual;
        private const string AssemblyName = "DynamicTinyMapper";
        protected abstract string ScopeName { get; }

        public bool IsSupported(TypePair typePair)
        {
            return IsSupportedCore(typePair);
        }

        protected string GetMapperFullName()
        {
            string random = Guid.NewGuid().ToString("N");
            return string.Format("{0}.{1}.Mapper{2}", AssemblyName, ScopeName, random);
        }

        protected abstract bool IsSupportedCore(TypePair typePair);
    }
}
