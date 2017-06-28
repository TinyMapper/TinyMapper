using System;
using Nelibur.ObjectMapper.Bindings;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers.Classes.Members;

namespace Nelibur.ObjectMapper.Mappers.Types.Custom
{
    internal sealed class CustomObjectMapperBuilder : MapperBuilder
    {
        public CustomObjectMapperBuilder(IMapperBuilderConfig config) : base(config)
        {
        }

        protected override string ScopeName
        {
            get { return "CustomObjectMapper"; }
        }


 

        protected override Mapper BuildCore(TypePair typePair)
        {
            Option<BindingConfig> bindingConfig = _config.GetBindingConfig(typePair);
            Option < Func <object, object>> converter = bindingConfig.Value.GetObjectConverter();
            return new CustomObjectMapper(converter.Value);
        }

        protected override Mapper BuildCore(TypePair parentTypePair, MappingMember mappingMember)
        {
            return BuildCore(mappingMember.TypePair);
        }

        protected override bool IsSupportedCore(TypePair typePair)
        {
            Option<BindingConfig> bindingConfig = _config.GetBindingConfig(typePair);

            return (bindingConfig.HasValue&&bindingConfig.Value.GetObjectConverter().HasValue);
        }
    }
}
