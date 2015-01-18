using System;
using TinyMapper.DataStructures;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers.Builders
{
    internal abstract class MapperBuilder : IMapperBuilder
    {
        protected readonly IDynamicAssembly _assembly;
        private const string Prefix = "TinyMapper";
        private readonly TargetMapperBuilder _targetMapperBuilder;

        protected MapperBuilder(IDynamicAssembly dynamicAssembly, TargetMapperBuilder targetMapperBuilder)
        {
            _assembly = dynamicAssembly;
            _targetMapperBuilder = targetMapperBuilder;
        }

        public Mapper Create(TypePair typePair)
        {
            return CreateCore(typePair);
        }

        public abstract bool IsSupported(TypePair typePair);
        protected abstract Mapper CreateCore(TypePair typePair);

        protected string GetMapperName(TypePair pair)
        {
            string random = Guid.NewGuid().ToString("N");
            return string.Format("{0}_{1}_{2}_{3}", Prefix, GetFullName(pair.Source), GetFullName(pair.Target), random);
        }

        private static string GetFullName(Type type)
        {
            return type == null ? "Empty" : type.FullName;
        }
    }
}
