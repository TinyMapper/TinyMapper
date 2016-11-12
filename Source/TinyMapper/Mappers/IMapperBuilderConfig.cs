using System;
using Nelibur.ObjectMapper.Bindings;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers.Classes.Members;
using Nelibur.ObjectMapper.Reflection;

namespace Nelibur.ObjectMapper.Mappers
{
    internal interface IMapperBuilderConfig
    {
        IDynamicAssembly Assembly { get; }
        Option<BindingConfig> GetBindingConfig(TypePair typePair);
        MapperBuilder GetMapperBuilder(TypePair typePair);
        MapperBuilder GetMapperBuilder(TypePair parentTypePair, MappingMember mappingMember);
        Func<string, string, bool> NameMatching { get; }
    }
}
