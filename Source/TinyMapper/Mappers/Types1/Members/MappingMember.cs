using System.Reflection;
using TinyMappers.DataStructures;
using TinyMappers.Extensions;

namespace TinyMappers.Mappers.Types1.Members
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
