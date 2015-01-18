using System.Reflection;
using TinyMapper.DataStructures;
using TinyMapper.Extensions;

namespace TinyMapper.Mappers.Builders.Members
{
    internal sealed class SimpleMappingMember : IMappingMember
    {
        public SimpleMappingMember(MemberInfo source, MemberInfo target)
        {
            Source = source;
            Target = target;
            TypePair = new TypePair(Source.GetMemberType(), Target.GetMemberType());
        }

        public MemberInfo Source { get; private set; }
        public MemberInfo Target { get; private set; }
        public TypePair TypePair { get; private set; }
    }
}
