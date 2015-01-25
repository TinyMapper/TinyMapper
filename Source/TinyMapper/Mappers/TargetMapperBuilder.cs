using System;
using System.Collections.Generic;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Builders;
using TinyMapper.Mappers.Classes;
using TinyMapper.Mappers.Types1;
using TinyMapper.Nelibur.Sword.Extensions;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers
{
    internal sealed class TargetMapperBuilder
    {
        private readonly IDynamicAssembly _assembly;
        //        private readonly List<MapperBuilder> _mapperBuilders;

        public TargetMapperBuilder(IDynamicAssembly assembly)
        {
            _assembly = assembly;
            //            _mapperBuilders = new List<MapperBuilder>
//            {
//                new ClassMapperBuilder(assembly, this)
//            };
        }

        public Mapper Build(TypePair typePair)
        {
            var mapper = ClassMapper.Create(_assembly, typePair);
            return mapper;

//            MappingType mappingType = MappingTypeBuilder.Build(typePair);
//            MapperBuilder mapperBuilder = GetMapperBuilder(mappingType.TypePair);
//            if (mapperBuilder.IsNull())
//            {
//                throw new NullReferenceException();
//            }
//            return mapperBuilder.Create(mappingType);
        }

//        private MapperBuilder GetMapperBuilder(TypePair typePair)
//        {
//            foreach (MapperBuilder mapperBuilder in _mapperBuilders)
//            {
//                if (mapperBuilder.IsSupported(typePair))
//                {
//                    return mapperBuilder;
//                }
//            }
//            return null;
//        }
    }
}
