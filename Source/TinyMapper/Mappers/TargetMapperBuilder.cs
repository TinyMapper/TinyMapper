using System;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers.Classes;
using Nelibur.ObjectMapper.Reflection;

namespace Nelibur.ObjectMapper.Mappers
{
    internal sealed class TargetMapperBuilder
    {
        private readonly ClassMapperBuilder _classMapperBuilder;

        public TargetMapperBuilder(IDynamicAssembly assembly)
        {
            _classMapperBuilder = new ClassMapperBuilder(assembly);
        }

        public Mapper Build(TypePair typePair)
        {
            Mapper mapper = _classMapperBuilder.Create(typePair);
            return mapper;
        }
    }
}
