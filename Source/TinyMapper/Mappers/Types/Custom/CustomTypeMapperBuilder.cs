using System;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers.Classes.Members;

namespace Nelibur.ObjectMapper.Mappers.Types.Custom
{
    internal sealed class CustomTypeMapperBuilder : MapperBuilder
    {
        public CustomTypeMapperBuilder(IMapperBuilderConfig config) : base(config)
        {
        }

        protected override string ScopeName
        {
            get { return "CustomTypeMapper"; }
        }

        protected override Mapper BuildCore(TypePair typePair)
        {
            throw new NotImplementedException();
        }

        protected override bool IsSupportedCore(TypePair typePair)
        {
            throw new NotSupportedException();
        }

        public bool IsSupported(MappingMember mappingMember)
        {
            var bindingConfig = _config.GetBindingConfig(mappingMember.TypePair);
            if (bindingConfig.HasNoValue)
            {
                return false;
            }
            return false;
        }
    }
}
