using System.Reflection;
using TinyMapper.DataStructures;
using TinyMapper.Extensions;

namespace TinyMapper.Mappers.Types.Members
{
    internal abstract class MappingMember
    {
        protected MappingMember(MemberInfo source, MemberInfo target)
        {
            Source = source;
            Target = target;
            if (source != null && target != null)
            {
                TypePair = new TypePair(Source.GetMemberType(), Target.GetMemberType());
            }
        }

        public bool IsRoot
        {
            get { return Source == null && Target == null; }
        }

        public MemberInfo Source { get; private set; }
        public MemberInfo Target { get; private set; }
        public TypePair TypePair { get; private set; }
    }
}
