using System;
using System.Collections.Generic;
using TinyMapper.Builders.Assemblies;
using TinyMapper.Builders.Assemblies.Types;

namespace TinyMapper
{
    public sealed class ObjectMapper
    {
        private static readonly IDynamicAssembly _assembly = DynamicAssemblyBuilder.Get();
        private static readonly Dictionary<MappingType, Mapper> _mappers = new Dictionary<MappingType, Mapper>();

        public static void Bind<TSource, TTarget>()
        {
            TargetMapperBuilder targetMapperBuilder = _assembly.GetTypeBuilder();
            Mapper mapper = targetMapperBuilder.Build(typeof(TSource), typeof(TTarget));

            _mappers[CreateMappingType(typeof(TSource), typeof(TTarget))] = mapper;
            _assembly.Save();
        }

        public static TTarget Project<TSource, TTarget>(TSource source, TTarget target)
        {
            Mapper mapper = _mappers[CreateMappingType(typeof(TSource), typeof(TTarget))];
            var result = (TTarget)mapper.MapMembers(source, target);

            return result;
        }

        public static TTarget Project<TTarget>(object source)
        {
            Mapper mapper = _mappers[CreateMappingType(source.GetType(), typeof(TTarget))];
            var result = (TTarget)mapper.MapMembers(source);
            return result;
        }

        private static MappingType CreateMappingType(Type source, Type target)
        {
            return new MappingType(source, target);
        }


        private sealed class MappingType
        {
            private readonly Type _source;
            private readonly Type _target;

            public MappingType(Type source, Type target)
            {
                _target = target;
                _source = source;
            }

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
                return obj is MappingType && Equals((MappingType)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((_source != null ? _source.GetHashCode() : 0) * 397) ^ (_target != null ? _target.GetHashCode() : 0);
                }
            }

            private bool Equals(MappingType other)
            {
                return _source == other._source && _target == other._target;
            }
        }
    }
}
