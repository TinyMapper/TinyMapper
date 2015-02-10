using System;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers.Classes;
using Nelibur.ObjectMapper.Mappers.Collections;
using Nelibur.ObjectMapper.Reflection;

namespace Nelibur.ObjectMapper.Mappers
{
    internal interface IMapperBuilderSelector
    {
        MapperBuilder GetMapperBuilder(TypePair typePair);
    }


    internal sealed class TargetMapperBuilder : IMapperBuilderSelector
    {
        private readonly ClassMapperBuilder _classMapperBuilder;
        private readonly CollectionMapperBuilder _collectionMapperBuilder;

        public TargetMapperBuilder(IDynamicAssembly assembly)
        {
            _classMapperBuilder = new ClassMapperBuilder(assembly, this);
            _collectionMapperBuilder = new CollectionMapperBuilder(assembly, this);
        }

        public Mapper Build(TypePair typePair)
        {
            Mapper mapper = _classMapperBuilder.Create(typePair);
            return mapper;
        }

        public MapperBuilder GetMapperBuilder(TypePair typePair)
        {
            if (_collectionMapperBuilder.IsSupported(typePair))
            {
                return _collectionMapperBuilder;
            }
            return _collectionMapperBuilder;
        }
    }
}
