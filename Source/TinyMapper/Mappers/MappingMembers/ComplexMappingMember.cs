using System;
using System.Reflection;

namespace Nelibur.ObjectMapper.Mappers.MappingMembers
{
    internal sealed class ComplexMappingMember : MappingMember
    {
        public ComplexMappingMember(MemberInfo source, MemberInfo target)
            : base(source, target)
        {
        }
    }
}
