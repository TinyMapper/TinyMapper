using System;
using Nelibur.ObjectMapper.Bindings;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers;
using Nelibur.ObjectMapper.Reflection;

namespace UnitTests
{
    internal class MappingBuilderConfigStub : IMapperBuilderConfig
    {
        public IDynamicAssembly Assembly
        {
            get { return DynamicAssemblyBuilder.Get(); }
        }

        public Option<BindingConfig> GetBindingConfig(TypePair typePair)
        {
            return Option<BindingConfig>.Empty;
        }

        public MapperBuilder GetMapperBuilder(TypePair typePair)
        {
            throw new NotImplementedException();
        }
    }
}
