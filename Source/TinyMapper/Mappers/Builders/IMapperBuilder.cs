using TinyMapper.DataStructures;

namespace TinyMapper.Mappers.Builders
{
    internal interface IMapperBuilder
    {
        bool IsSupported(TypePair typePair);
    }
}