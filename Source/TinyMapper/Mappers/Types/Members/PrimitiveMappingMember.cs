using System.Reflection;

namespace TinyMapper.Mappers.Types.Members
{
    internal sealed class PrimitiveMappingMember : MappingMember
    {
        public PrimitiveMappingMember(MemberInfo source, MemberInfo target) : base(source, target)
        {
        }
    }
}
