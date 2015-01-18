using System;
using System.Collections.Generic;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Builders;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers
{
    internal sealed class TargetMapperBuilder
    {
        private readonly List<MapperBuilder> _mapperBuilders;

        public TargetMapperBuilder(IDynamicAssembly assembly)
        {
            _mapperBuilders = new List<MapperBuilder>
            {
                new PrimitiveTypeMapperBuilder(assembly, this),
                new CollectionMapperBuilder(assembly, this),
                new ClassMapperBuilder(assembly, this)
            };
        }

        public Mapper Build(TypePair typePair)
        {
            foreach (MapperBuilder mapperBuilder in _mapperBuilders)
            {
                if (mapperBuilder.IsSupported(typePair))
                {
                    return mapperBuilder.Create(typePair);
                }
            }
            throw new NotSupportedException();
        }
    }
}
