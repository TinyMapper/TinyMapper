using System.Collections.Generic;
using System.Reflection;

namespace TinyMapper.Mappers.Types.Members
{
    internal sealed class CompositeMappingMember : MappingMember
    {
        private readonly List<MappingMember> _members = new List<MappingMember>();

        public CompositeMappingMember(MemberInfo source, MemberInfo target) : base(source, target)
        {
        }

        public CompositeMappingMember() : this(null, null)
        {
        }

        public IEnumerable<MappingMember> Members
        {
            get { return _members; }
        }

        public void Add(MappingMember member)
        {
            _members.Add(member);
        }
    }
}
