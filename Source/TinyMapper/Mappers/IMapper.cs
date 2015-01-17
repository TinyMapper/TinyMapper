using TinyMapper.DataStructures;

namespace TinyMapper.Mappers
{
    internal interface IMapper
    {
        bool IsSupported(TypePair typePair);
    }
}