using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper.Bindings;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers.Classes;
using Nelibur.ObjectMapper.Mappers.Collections;
using Nelibur.ObjectMapper.Mappers.Types.Convertible;
using Nelibur.ObjectMapper.Reflection;

namespace Nelibur.ObjectMapper.Mappers
{
    internal sealed class TargetMapperBuilder : IMapperBuilderConfig
    {
        private readonly Dictionary<TypePair, BindingConfig> _bindingConfigs = new Dictionary<TypePair, BindingConfig>();
        private readonly ClassMapperBuilder _classMapperBuilder;
        private readonly CollectionMapperBuilder _collectionMapperBuilder;
        private readonly ConvertibleTypeMapperBuilder _convertibleTypeMapperBuilder;

        public TargetMapperBuilder(IDynamicAssembly assembly)
        {
            Assembly = assembly;

            _classMapperBuilder = new ClassMapperBuilder(this);
            _collectionMapperBuilder = new CollectionMapperBuilder(this);
            _convertibleTypeMapperBuilder = new ConvertibleTypeMapperBuilder(this);
        }

        public IDynamicAssembly Assembly { get; private set; }

        public Option<BindingConfig> GetBindingConfig(TypePair typePair)
        {
            BindingConfig result;
            bool exists = _bindingConfigs.TryGetValue(typePair, out result);
            return new Option<BindingConfig>(result, exists);
        }

        public MapperBuilder GetMapperBuilder(TypePair typePair)
        {
            if (_convertibleTypeMapperBuilder.IsSupported(typePair))
            {
                return _convertibleTypeMapperBuilder;
            }
            else if (_collectionMapperBuilder.IsSupported(typePair))
            {
                return _collectionMapperBuilder;
            }
            return _classMapperBuilder;
        }

        public Mapper Build(TypePair typePair, BindingConfig bindingConfig)
        {
            _bindingConfigs[typePair] = bindingConfig;
            return Build(typePair);
        }

        public Mapper Build(TypePair typePair)
        {
            MapperBuilder mapperBuilder = GetMapperBuilder(typePair);
            Mapper mapper = mapperBuilder.Build(typePair);
            return mapper;
        }
    }
}
