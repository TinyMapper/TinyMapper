using System.Reflection;

namespace TinyMapper.Mappers.Types.Members
{
    internal sealed class ComplexMappingMember : MappingMember
    {
        public ComplexMappingMember(MemberInfo source, MemberInfo target)
            : base(source, target)
        {
        }
    }
}
