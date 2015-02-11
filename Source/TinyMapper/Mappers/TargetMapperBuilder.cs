using System;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers.Classes;
using Nelibur.ObjectMapper.Mappers.Collections;
using Nelibur.ObjectMapper.Reflection;

namespace Nelibur.ObjectMapper.Mappers
{
    internal sealed class TargetMapperBuilder : IMapperBuilderConfig
    {
        private readonly ClassMapperBuilder _classMapperBuilder;
        private readonly CollectionMapperBuilder _collectionMapperBuilder;

        public TargetMapperBuilder(IDynamicAssembly assembly)
        {
            Assembly = assembly;

            _classMapperBuilder = new ClassMapperBuilder(this);
            _collectionMapperBuilder = new CollectionMapperBuilder(this);
        }

        public IDynamicAssembly Assembly { get; private set; }

        public Mapper Build(TypePair typePair)
        {
            MapperBuilder mapperBuilder = GetMapperBuilder(typePair);
            Mapper mapper = mapperBuilder.Create(typePair);
            return mapper;
        }

        public MapperBuilder GetMapperBuilder(TypePair typePair)
        {
            if (_collectionMapperBuilder.IsSupported(typePair))
            {
                return _collectionMapperBuilder;
            }
            return _classMapperBuilder;
        }
    }
}
