using System.Reflection;

namespace TinyMapper.Mappers.Builders.Members
{
    internal sealed class SimpleMappingMember : MappingMember
    {
        public SimpleMappingMember(MemberInfo source, MemberInfo target) : base(source, target)
        {
        }
    }
}
