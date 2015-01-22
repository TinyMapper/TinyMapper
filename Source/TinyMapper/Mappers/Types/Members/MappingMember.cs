using System;
using System.Reflection;
using TinyMapper.DataStructures;
using TinyMapper.Extensions;

namespace TinyMapper.Mappers.Types.Members
{
    internal abstract class MappingMember : IEquatable<MappingMember>
    {
        protected MappingMember(MemberInfo source, MemberInfo target)
        {
            Source = source;
            Target = target;
            SourceName = Source.Name;
            TargetName = Target.Name;
            TypePair = new TypePair(Source.GetMemberType(), Target.GetMemberType());
        }

        public MemberInfo Source { get; private set; }
        public string SourceName { get; private set; }
        public MemberInfo Target { get; private set; }
        public string TargetName { get; private set; }
        public TypePair TypePair { get; private set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((MappingMember)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (SourceName != null ? SourceName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TargetName != null ? TargetName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ TypePair.GetHashCode();
                return hashCode;
            }
        }

        public bool Equals(MappingMember other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(SourceName, other.SourceName)
                   && string.Equals(TargetName, other.TargetName)
                   && TypePair.Equals(other.TypePair);
        }
    }
}
