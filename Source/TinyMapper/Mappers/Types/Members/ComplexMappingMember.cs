using System;
using System.Collections.Generic;
using System.Reflection;
using TinyMapper.DataStructures;
using TinyMapper.Extensions;

namespace TinyMapper.Mappers.Types.Members
{
    internal sealed class ComplexMappingMember : MappingMember
    {
        private readonly List<MappingMember> _members = new List<MappingMember>();

        public ComplexMappingMember(MemberInfo source, MemberInfo target)
            : base(source, target)
        {
            TypePair = new TypePair(Source.GetMemberType(), Target.GetMemberType());
        }

        public ComplexMappingMember(TypePair typePair)
            : base(null, null)
        {
            TypePair = typePair;
        }

        public IEnumerable<MappingMember> Members
        {
            get { return _members; }
        }

        public override TypePair TypePair { get; protected set; }

        public void Add(MappingMember member)
        {
            _members.Add(member);
        }
    }
}
