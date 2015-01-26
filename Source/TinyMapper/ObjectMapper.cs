using System;
using System.Collections.Generic;
using TinyMapper.DataStructures;
using TinyMapper.Mappers;
using TinyMapper.Nelibur.Sword.Core;
using TinyMapper.Nelibur.Sword.Extensions;
using TinyMapper.Reflection;

namespace TinyMapper
{
    public sealed class ObjectMapper
    {
        private static readonly IDynamicAssembly _assembly = DynamicAssemblyBuilder.Get();
        private static readonly Dictionary<TypePair, Mapper> _mappers = new Dictionary<TypePair, Mapper>();

        public static void Bind<TSource, TTarget>()
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();
            _mappers[typePair] = CreateMapper(typePair);

            _assembly.Save();
        }

        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target)
        {
            if (source.IsNull())
            {
                throw Error.ArgumentNull("source");
            }

            if (target.IsNull())
            {
                throw Error.ArgumentNull("target");
            }

            TypePair typePair = TypePair.Create<TSource, TTarget>();

            Mapper mapper = GetMapper(typePair);
            var result = (TTarget)mapper.Map(source, target);

            return result;
        }

        public static TTarget Map<TTarget>(object source)
        {
            if (source.IsNull())
            {
                throw Error.ArgumentNull("source");
            }

            TypePair typePair = TypePair.Create(source.GetType(), typeof(TTarget));

            Mapper mapper = GetMapper(typePair);
            var result = (TTarget)mapper.Map(source);

            return result;
        }

        private static Mapper CreateMapper(TypePair typePair)
        {
            TargetMapperBuilder targetMapperBuilder = _assembly.GetTypeBuilder();
            Mapper mapper = targetMapperBuilder.Build(typePair);
            return mapper;
        }

        private static Mapper GetMapper(TypePair typePair)
        {
            Mapper mapper;
            if (_mappers.TryGetValue(typePair, out mapper) == false)
            {
                mapper = CreateMapper(typePair);
                _mappers[typePair] = mapper;
            }
            return mapper;
        }
    }
}
