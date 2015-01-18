using System.Collections.Generic;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Types.Members;

namespace TinyMapper.Mappers.Types
{
    internal sealed class MappingType
    {
        public MappingType(TypePair typePair)
        {
            TypePair = typePair;
            Members = new List<MappingMember>();
        }

        public List<MappingMember> Members { get; private set; }
        public TypePair TypePair { get; private set; }

        public MappingType AddMembers(List<MappingMember> members)
        {
            Members.AddRange(members);
            return this;
        }
    }
}
