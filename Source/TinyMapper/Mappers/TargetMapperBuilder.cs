using TinyMappers.DataStructures;
using TinyMappers.Mappers.Classes;
using TinyMappers.Reflection;

namespace TinyMappers.Mappers
{
    internal sealed class TargetMapperBuilder
    {
        private readonly IDynamicAssembly _assembly;

        public TargetMapperBuilder(IDynamicAssembly assembly)
        {
            _assembly = assembly;
        }

        public Mapper Build(TypePair typePair)
        {
            Mapper mapper = ClassMapper.Create(_assembly, typePair);
            return mapper;
        }
    }
}
