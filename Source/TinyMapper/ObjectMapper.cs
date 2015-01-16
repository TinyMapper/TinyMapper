using System;
using System.Collections.Generic;
using TinyMapper.Builders.Assemblies;
using TinyMapper.Builders.Assemblies.Types;
using TinyMapper.DataStructures;

namespace TinyMapper
{
    public sealed class ObjectMapper
    {
        private static readonly IDynamicAssembly _assembly = DynamicAssemblyBuilder.Get();
        private static readonly Dictionary<TypePair, Mapper> _mappers = new Dictionary<TypePair, Mapper>();

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

        private static TypePair CreateMappingType(Type source, Type target)
        {
            return new TypePair(source, target);
        }
    }
}
