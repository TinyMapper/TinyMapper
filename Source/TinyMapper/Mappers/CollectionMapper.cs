using System.Collections;
using TinyMapper.DataStructures;

namespace TinyMapper.Mappers
{
    internal sealed class CollectionMapper : IMapper
    {
        public bool IsSupported(TypePair typePair)
        {
            return typePair.Target.IsArray
                   || typeof(IEnumerable).IsAssignableFrom(typePair.Target);
        }
    }
}
