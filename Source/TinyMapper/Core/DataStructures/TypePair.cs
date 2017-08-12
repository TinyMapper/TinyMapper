using System;
using System.ComponentModel;
using Nelibur.ObjectMapper.Core.Extensions;

namespace Nelibur.ObjectMapper.Core.DataStructures
{
    internal struct TypePair : IEquatable<TypePair>
    {
        public TypePair(Type source, Type target) : this()
        {
            Target = target;
            Source = source;
        }

        public bool IsDeepCloneable
        {
            get
            {
                if (IsEqualTypes == false)
                {
                    return false;
                }
                else if (IsValueTypes && IsPrimitiveTypes)
                {
                    return true;
                }
                else if (Source == typeof(string) || Source == typeof(decimal) ||
                         Source == typeof(DateTime) || Source == typeof(DateTimeOffset) ||
                         Source == typeof(TimeSpan) || Source == typeof(Guid))
                {
                    return true;
                }
                else if (IsNullableTypes)
                {
                    var nullablePair = new TypePair(Nullable.GetUnderlyingType(Source), Nullable.GetUnderlyingType(Target));
                    return nullablePair.IsDeepCloneable;
                }
                return false;
            }
        }

        public bool IsEnumTypes => Helpers.IsEnum(Source) && Helpers.IsEnum(Target);

        public bool IsEnumerableTypes => Source.IsIEnumerable() && Target.IsIEnumerable();

        public bool IsNullableToNotNullable => Source.IsNullable() && Target.IsNullable() == false;

        public Type Source { get; }
        public Type Target { get; }

        private bool IsEqualTypes => Source == Target;

        private bool IsNullableTypes => Source.IsNullable() && Target.IsNullable();

        private bool IsPrimitiveTypes => Helpers.IsPrimitive(Source) && Helpers.IsPrimitive(Target);

        private bool IsValueTypes => Helpers.IsValueType(Source) && Helpers.IsValueType(Target);

        public static TypePair Create(Type source, Type target)
        {
            return new TypePair(source, target);
        }

        public static TypePair Create<TSource, TTarget>()
        {
            return new TypePair(typeof(TSource), typeof(TTarget));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is TypePair && Equals((TypePair)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Source != null ? Source.GetHashCode() : 0) * 397) ^ (Target != null ? Target.GetHashCode() : 0);
            }
        }

        // TODO Cache TypeConverters
        public bool HasTypeConverter()
        {
            TypeConverter fromConverter = TypeDescriptor.GetConverter(Source);
            if (fromConverter.CanConvertTo(Target))
            {
                return true;
            }

            TypeConverter toConverter = TypeDescriptor.GetConverter(Target);
            if (toConverter.CanConvertFrom(Source))
            {
                return true;
            }
            return false;
        }

        public bool Equals(TypePair other)
        {
            return Source == other.Source && Target == other.Target;
        }
    }
}
