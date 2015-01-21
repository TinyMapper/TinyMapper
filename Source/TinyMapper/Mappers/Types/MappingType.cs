using System.Collections.Generic;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Types.Members;

namespace TinyMapper.Mappers.Types
{
    internal sealed class MappingType
    {
        public MappingType(TypePair typePair)
        {
            Members = new List<MappingMember>();
            TypePair = typePair;
        }

        public List<MappingMember> Members { get; private set; }

        public TypePair TypePair { get; private set; }
    }
}
