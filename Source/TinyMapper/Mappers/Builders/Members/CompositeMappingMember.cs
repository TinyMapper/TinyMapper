using System.Collections.Generic;
using System.Reflection;
using TinyMapper.DataStructures;
using TinyMapper.Extensions;

namespace TinyMapper.Mappers.Builders.Members
{
    internal sealed class CompositeMappingMember : IMappingMember
    {
        private readonly List<IMappingMember> _members = new List<IMappingMember>();

        public CompositeMappingMember(MemberInfo source, MemberInfo target)
        {
            Source = source;
            Target = target;
            TypePair = new TypePair(Source.GetMemberType(), Target.GetMemberType());
        }

        public MemberInfo Source { get; private set; }
        public MemberInfo Target { get; private set; }
        public TypePair TypePair { get; private set; }

        public void Add(IMappingMember member)
        {
            _members.Add(member);
        }

        public void AddRange(List<IMappingMember> members)
        {
            _members.AddRange(members);
        }
    }
}
