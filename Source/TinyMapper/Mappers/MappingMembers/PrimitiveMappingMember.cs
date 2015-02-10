using System;
using System.Reflection;

namespace Nelibur.ObjectMapper.Mappers.MappingMembers
{
    internal sealed class PrimitiveMappingMember : MappingMember
    {
        public PrimitiveMappingMember(MemberInfo source, MemberInfo target) : base(source, target)
        {
        }
    }
}
