using System.Collections;
using TinyMapper.DataStructures;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers.Builders
{
    internal sealed class CollectionMapperBuilder : IMapperBuilder
    {
        private IDynamicAssembly _dynamicAssembly;

        public CollectionMapperBuilder(IDynamicAssembly dynamicAssembly)
        {
            _dynamicAssembly = dynamicAssembly;
        }

        public bool IsSupported(TypePair typePair)
        {
            return typePair.Target.IsArray
                   || typeof(IEnumerable).IsAssignableFrom(typePair.Target);
        }
    }
}
