using System;
using System.Reflection;

namespace Nelibur.ObjectMapper.Mappers.MappingTypes.Members
{
    internal sealed class ComplexMappingMember : MappingMember
    {
        public ComplexMappingMember(MemberInfo source, MemberInfo target)
            : base(source, target)
        {
        }
    }
}
