using System.Reflection;
using Nelibur.Mapper.Core.DataStructures;
using Nelibur.Mapper.Core.Extensions;

namespace Nelibur.Mapper.Mappers.Types1.Members
{
    internal abstract class MappingMember
    {
        protected MappingMember(MemberInfo source, MemberInfo target)
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
