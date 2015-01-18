using TinyMapper.DataStructures;
using TinyMapper.Mappers.Types.Members;

namespace TinyMapper.Mappers.Types
{
    internal sealed class MappingType
    {
        private readonly CompositeMappingMember _rootMember = new CompositeMappingMember();

        public MappingType(TypePair typePair)
        {
            TypePair = typePair;
        }

        public CompositeMappingMember RootMember
        {
            get { return _rootMember; }
        }

        public TypePair TypePair { get; private set; }

        public void AddMember(MappingMember member)
        {
            _rootMember.Add(member);
        }
    }
}
