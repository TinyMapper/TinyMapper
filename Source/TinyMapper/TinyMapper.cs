using System.Collections.Generic;
using Nelibur.Mapper.Core;
using Nelibur.Mapper.Core.DataStructures;
using Nelibur.Mapper.Core.Extensions;
using Nelibur.Mapper.Mappers;
using Nelibur.Mapper.Reflection;

namespace Nelibur.Mapper
{
    public sealed class TinyMapper
    {
        private static readonly IDynamicAssembly _assembly = DynamicAssemblyBuilder.Get();
        private static readonly Dictionary<TypePair, Mappers.Mapper> _mappers = new Dictionary<TypePair, Mappers.Mapper>();

        public static void Bind<TSource, TTarget>()
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();
            _mappers[typePair] = CreateMapper(typePair);

            _assembly.Save();
        }

        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target = default(TTarget))
        {
            if (source.IsNull())
            {
                throw Error.ArgumentNull("source");
            }

            TypePair typePair = TypePair.Create<TSource, TTarget>();

            Mappers.Mapper mapper = GetMapper(typePair);
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

            Mappers.Mapper mapper = GetMapper(typePair);
            var result = (TTarget)mapper.Map(source);

            return result;
        }

        private static Mappers.Mapper CreateMapper(TypePair typePair)
        {
            TargetMapperBuilder targetMapperBuilder = _assembly.GetTypeBuilder();
            Mappers.Mapper mapper = targetMapperBuilder.Build(typePair);
            return mapper;
        }

        private static Mappers.Mapper GetMapper(TypePair typePair)
        {
            Mappers.Mapper mapper;
            if (_mappers.TryGetValue(typePair, out mapper) == false)
            {
                mapper = CreateMapper(typePair);
                _mappers[typePair] = mapper;
            }
            return mapper;
        }
    }
}
