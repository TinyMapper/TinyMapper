using System;
using Nelibur.ObjectMapper.Bindings;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;
using Nelibur.ObjectMapper.Mappers;
using Nelibur.ObjectMapper.Mappers.Classes.Members;
using Nelibur.ObjectMapper.Reflection;

namespace UnitTests
{
    internal class MappingBuilderConfigStub : IMapperBuilderConfig
    {
        private readonly Option<BindingConfig> _bindingConfig = Option<BindingConfig>.Empty;

        public MappingBuilderConfigStub()
        {
        }

        public MappingBuilderConfigStub(BindingConfig bindingConfig)
        {
            _bindingConfig = bindingConfig.ToOption();
        }

        public IDynamicAssembly Assembly
        {
            get { return DynamicAssemblyBuilder.Get(); }
        }

        public Option<BindingConfig> GetBindingConfig(TypePair typePair)
        {
            return _bindingConfig;
        }

        public MapperBuilder GetMapperBuilder(MappingMember mappingMember)
        {
            throw new NotImplementedException();
        }

        public MapperBuilder GetMapperBuilder(TypePair typePair)
        {
            throw new NotImplementedException();
        }
    }
}
