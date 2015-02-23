using System;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers;
using Nelibur.ObjectMapper.Reflection;

namespace UnitTests.Mappers.Classes
{
    internal class MappingBuilderConfigStub : IMapperBuilderConfig
    {
        public IDynamicAssembly Assembly
        {
            get { return DynamicAssemblyBuilder.Get(); }
        }

        public MapperBuilder GetMapperBuilder(TypePair typePair)
        {
            throw new NotImplementedException();
        }
    }
}
