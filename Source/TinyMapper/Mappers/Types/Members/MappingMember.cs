using System.Reflection;
using TinyMapper.DataStructures;

namespace TinyMapper.Mappers.Types.Members
{
    internal abstract class MappingMember
    {
        protected MappingMember(MemberInfo source, MemberInfo target)
        {
            Source = source;
            Target = target;
        }

        public bool IsRoot
        {
            get { return Source == null && Target == null; }
        }

        public MemberInfo Source { get; private set; }
        public MemberInfo Target { get; private set; }
        public abstract TypePair TypePair { get; protected set; }
    }
}
