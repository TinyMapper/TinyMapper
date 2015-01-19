using System;
using System.Collections.Generic;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Builders;
using TinyMapper.Mappers.Types;
using TinyMapper.Mappers.Types.Members;
using TinyMapper.Nelibur.Sword.Extensions;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers
{
    internal sealed class TargetMapperBuilder
    {
        private readonly List<MapperBuilder> _mapperBuilders;
        private readonly MappingTypeBuilder _mappingTypeBuilder = new MappingTypeBuilder();

        public TargetMapperBuilder(IDynamicAssembly assembly)
        {
            _mapperBuilders = new List<MapperBuilder>
            {
                new CollectionMapperBuilder(assembly, this),
                new ClassMapperBuilder(assembly, this)
            };
        }

        public Mapper Build(TypePair typePair)
        {
            CompositeMappingMember member = _mappingTypeBuilder.Build(typePair);
            MapperBuilder mapperBuilder = GetMapperBuilder(member);
            if (mapperBuilder.IsNull())
            {
                throw new NullReferenceException();
            }
            return mapperBuilder.Create(member);
        }

        private MapperBuilder GetMapperBuilder(CompositeMappingMember mappingType)
        {
            foreach (MapperBuilder mapperBuilder in _mapperBuilders)
            {
                if (mapperBuilder.IsSupported(mappingType.TypePair))
                {
                    return mapperBuilder;
                }
            }
            return null;
        }
    }
}
