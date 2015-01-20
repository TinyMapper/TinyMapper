using TinyMapper.DataStructures;
using TinyMapper.Mappers.Types;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers.Builders
{
    internal abstract class MapperBuilder
    {
        protected readonly IDynamicAssembly _assembly;
        private readonly TargetMapperBuilder _targetMapperBuilder;

        protected MapperBuilder(IDynamicAssembly dynamicAssembly, TargetMapperBuilder targetMapperBuilder)
        {
            _assembly = dynamicAssembly;
            _targetMapperBuilder = targetMapperBuilder;
        }

        public Mapper Create(MappingType mappingType)
        {
            return CreateCore(mappingType);
        }

        public abstract bool IsSupported(TypePair typePair);
        protected abstract Mapper CreateCore(MappingType mappingType);
    }
}
