using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper.Bindings;
using Nelibur.ObjectMapper.Core;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;
using Nelibur.ObjectMapper.Mappers;
using Nelibur.ObjectMapper.Reflection;
using System.Threading;

namespace Nelibur.ObjectMapper
{
    public static class TinyMapper
    {
        private static readonly Dictionary<TypePair, Mapper> _mappers = new Dictionary<TypePair, Mapper>();
        private static readonly TargetMapperBuilder _targetMapperBuilder;
        private static readonly TinyMapperConfig _config;
        private static readonly ReaderWriterLockSlim _mappersLock = new ReaderWriterLockSlim();

        static TinyMapper()
        {
            IDynamicAssembly assembly = DynamicAssemblyBuilder.Get();
            _targetMapperBuilder = new TargetMapperBuilder(assembly);
            _config = new TinyMapperConfig(_targetMapperBuilder);
        }

        public static void Bind<TSource, TTarget>()
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();

            _mappersLock.EnterReadLock();
            _mappers[typePair] = _targetMapperBuilder.Build(typePair);
            _mappersLock.ExitReadLock();
        }

        public static void Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config)
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();

            var bindingConfig = new BindingConfigOf<TSource, TTarget>();
            config(bindingConfig);

            _mappersLock.EnterReadLock();
            _mappers[typePair] = _targetMapperBuilder.Build(typePair, bindingConfig);
            _mappersLock.ExitReadLock();
        }

        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target = default(TTarget))
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();

            Mapper mapper = GetMapper(typePair);
            var result = (TTarget)mapper.Map(source, target);

            return result;
        }

        public static void Config(Action<ITinyMapperConfig> config)
        {
            config(_config);
        }

        /// <summary>
        ///     Maps the specified source to <see cref="TTarget" /> type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source value.</param>
        /// <returns>Value</returns>
        /// <remarks>For mapping nullable type use <see cref="Map{TTarget}" />method.</remarks>
        public static TTarget Map<TTarget>(object source)
        {
            if (source.IsNull())
            {
                throw Error.ArgumentNull("source, for mapping nullable type use Map<TSource, TTarget> method");
            }

            TypePair typePair = TypePair.Create(source.GetType(), typeof(TTarget));

            Mapper mapper = GetMapper(typePair);
            var result = (TTarget)mapper.Map(source);

            return result;
        }

        private static Mapper GetMapper(TypePair typePair)
        {
            Mapper mapper;
            _mappersLock.EnterUpgradeableReadLock();
            try
            {
                if (_mappers.TryGetValue(typePair, out mapper) == false)
                {
                    mapper = _targetMapperBuilder.Build(typePair);
                    _mappersLock.EnterWriteLock();
                    _mappers[typePair] = mapper;
                    _mappersLock.ExitWriteLock();
                }
            }
            finally
            {
                _mappersLock.ExitUpgradeableReadLock();
            }

            return mapper;
        }
    }
}
