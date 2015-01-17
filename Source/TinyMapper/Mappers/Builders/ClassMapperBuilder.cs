using TinyMapper.DataStructures;

namespace TinyMapper.Mappers.Builders
{
    internal sealed class ClassMapperBuilder : IMapperBuilder
    {
        public bool IsSupported(TypePair typePair)
        {
            return true;
        }
    }
}
