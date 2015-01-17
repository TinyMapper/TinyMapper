using TinyMapper.DataStructures;

namespace TinyMapper.Mappers
{
    internal sealed class ClassMapper : IMapper
    {
        public bool IsSupported(TypePair typePair)
        {
            return true;
        }
    }
}
