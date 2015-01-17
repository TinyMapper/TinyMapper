using System.Reflection;

namespace TinyMapper.Mappers.Builders.Members
{
    internal sealed class MappingMember
    {
        public MappingMember(MemberInfo source, MemberInfo target)
        {
            Source = source;
            Target = target;
        }

        public MemberInfo Source { get; private set; }
        public MemberInfo Target { get; private set; }
    }
}
