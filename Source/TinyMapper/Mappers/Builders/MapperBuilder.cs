using TinyMapper.DataStructures;
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

        public Mapper Create(TypePair typePair)
        {
            return CreateCore(typePair);
        }

        public abstract bool IsSupported(TypePair typePair);
        protected abstract Mapper CreateCore(TypePair typePair);
    }
}
