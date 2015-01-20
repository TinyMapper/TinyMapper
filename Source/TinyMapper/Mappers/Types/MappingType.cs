using System.Collections.Generic;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Types.Members;

namespace TinyMapper.Mappers.Types
{
    internal sealed class MappingType
    {
        private readonly List<MappingMember> _members = new List<MappingMember>();

        public MappingType(TypePair typePair)
        {
            TypePair = typePair;
        }

        public IEnumerable<MappingMember> Members
        {
            get { return _members; }
        }

        public TypePair TypePair { get; private set; }

        public void AddMember(MappingMember member)
        {
            _members.Add(member);
        }
    }
}
