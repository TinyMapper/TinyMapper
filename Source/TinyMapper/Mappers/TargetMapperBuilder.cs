using System;
using Nelibur.Mapper.Core.DataStructures;
using Nelibur.Mapper.Mappers.Classes;
using Nelibur.Mapper.Reflection;

namespace Nelibur.Mapper.Mappers
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
