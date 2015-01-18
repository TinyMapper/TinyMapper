using System.Collections.Generic;
using System.Reflection;

namespace TinyMapper.Mappers.Builders.Members
{
    internal sealed class CompositeMappingMember : MappingMember
    {
        private readonly List<MappingMember> _members = new List<MappingMember>();

        public CompositeMappingMember(MemberInfo source, MemberInfo target) : base(source, target)
        {
        }

        public void AddRange(List<MappingMember> members)
        {
            _members.AddRange(members);
        }
    }
}
