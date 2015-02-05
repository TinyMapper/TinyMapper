using System;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers.Classes;
using Nelibur.ObjectMapper.Reflection;

namespace Nelibur.ObjectMapper.Mappers
{
    internal sealed class TargetMapperBuilder
    {
        private readonly IDynamicAssembly _assembly;

        public TargetMapperBuilder(IDynamicAssembly assembly)
        {
            _assembly = assembly;
        }

        public Mapper Build(TypePair typePair)
        {
            Mapper mapper = ClassMapperBuilder.Create(_assembly, typePair);
            return mapper;
        }
    }
}
