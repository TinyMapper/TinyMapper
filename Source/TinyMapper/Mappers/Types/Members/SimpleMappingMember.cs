using System.Reflection;
using TinyMapper.DataStructures;
using TinyMapper.Extensions;

namespace TinyMapper.Mappers.Types.Members
{
    internal sealed class SimpleMappingMember : MappingMember
    {
        public SimpleMappingMember(MemberInfo source, MemberInfo target) : base(source, target)
        {
            TypePair = new TypePair(Source.GetMemberType(), Target.GetMemberType());
        }

        public override TypePair TypePair { get; protected set; }
    }
}
