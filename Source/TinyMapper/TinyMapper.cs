using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nelibur.ObjectMapper.Bindings;
using Nelibur.ObjectMapper.Core;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;
using Nelibur.ObjectMapper.Mappers;
using Nelibur.ObjectMapper.Reflection;

namespace Nelibur.ObjectMapper
{
    public interface IBindingConfig<TTarget>
    {
        void Ignore(Expression<Func<TTarget, object>> expression);
    }


    internal sealed class BindingConfig<TTarget> : IBindingConfig<TTarget>
    {
        public void Ignore(Expression<Func<TTarget, object>> expression)
        {
            Console.WriteLine("1");
            //            throw new NotImplementedException();
        }
    }

    public static class TinyMapper
    {
        private static readonly Dictionary<TypePair, Mapper> _mappers = new Dictionary<TypePair, Mapper>();
        private static readonly TargetMapperBuilder _targetMapperBuilder;

        static TinyMapper()
        {
            IDynamicAssembly assembly = DynamicAssemblyBuilder.Get();
            _targetMapperBuilder = new TargetMapperBuilder(assembly);
        }

        public static void Bind<TSource, TTarget>()
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();
            _mappers[typePair] = CreateMapper(typePair);
        }

        public static void Bind<TSource, TTarget>(Action<IBindingConfig<TTarget>> config)
        {
            Console.WriteLine("0");
            var bindingConfig = new BindingConfig<TTarget>();
            config(bindingConfig);
            TypePair typePair = TypePair.Create<TSource, TTarget>();
            _mappers[typePair] = CreateMapper(typePair);
        }

        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target = default(TTarget))
        {
            if (source.IsNull())
            {
                throw Error.ArgumentNull("source");
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
            Mapper mapper = _targetMapperBuilder.Build(typePair);
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
