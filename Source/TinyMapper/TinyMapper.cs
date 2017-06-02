using System;
using System.Collections.Generic;
using System.Threading;
using Nelibur.ObjectMapper.Bindings;
using Nelibur.ObjectMapper.Core;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;
using Nelibur.ObjectMapper.Mappers;
using Nelibur.ObjectMapper.Reflection;

namespace Nelibur.ObjectMapper
{
    /// <summary>
    ///     TinyMapper is an object to object mapper for .NET. The main advantage is performance.
    ///     TinyMapper allows easily map object to object, i.e. properties or fields from one
    ///     object to another, for instance.
    /// </summary>
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

        /// <summary>
        ///     Create a one-way mapping between one type and another
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TTarget">Target type</typeparam>
        public static void Bind<TSource, TTarget>()
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();
            _mappersLock.EnterWriteLock();
            try
            {
                _mappers[typePair] = _targetMapperBuilder.Build(typePair);
            }
            finally
            {
                _mappersLock.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Create a one-way mapping between one type and another
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TTarget">Target type</typeparam>
        /// <param name="config">BindingConfig for Custom Binding</param>
        public static void Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config)
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();

            var bindingConfig = new BindingConfigOf<TSource, TTarget>();
            config(bindingConfig);

            _mappersLock.EnterWriteLock();
            try
            {
                _mappers[typePair] = _targetMapperBuilder.Build(typePair, bindingConfig);
            }
            finally
            {
                _mappersLock.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Find out if a binding exists for Source to Target
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TTarget">Target type</typeparam>
        /// <returns></returns>
        public static bool BindingExists<TSource, TTarget>()
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();
            Mapper mapper;

            _mappersLock.EnterReadLock();
            var result = _mappers.TryGetValue(typePair, out mapper);
            _mappersLock.ExitReadLock();

            return result;
        }

        /// <summary>
        ///     Coppies data from one class to another
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TTarget">Target type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="target">Target object (or null)</param>
        /// <returns>Mapped object</returns>
        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target = default(TTarget))
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();

            Mapper mapper = GetMapper(typePair);
            var result = (TTarget)mapper.Map(source, target);

            return result;
        }

        /// <summary>
        ///     Configure the Mapper
        /// </summary>
        /// <param name="config">Lambda to provide config settings</param>
        public static void Config(Action<ITinyMapperConfig> config)
        {
            config(_config);
        }

        /// <summary>
        ///     Maps the specified source to TTarget /> type.
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
                    if (_config.EnablePolymorphicMapping && (mapper = GetPolymorphicMapping(typePair)) != null)
                    {
                        return mapper;
                    }
                    else if (_config.EnableAutoBinding)
                    {
                        mapper = _targetMapperBuilder.Build(typePair);
                        _mappersLock.EnterWriteLock();
                        try
                        {
                            _mappers[typePair] = mapper;
                        }
                        finally
                        {
                            _mappersLock.ExitWriteLock();
                        }
                    }
                    else
                        throw new MappingException($"Unable to find a binding for type '{typePair.Source?.Name}' to '{typePair.Target?.Name}'.");
                }
            }
            finally
            {
                _mappersLock.ExitUpgradeableReadLock();
            }

            return mapper;
        }

        //Note: Lock should already be acquired for the mapper
        private static Mapper GetPolymorphicMapping(TypePair types)
        {
            // Walk the polymorphic heirarchy until we find a mapping match
            Type source = types.Source;

            do
            {
                Mapper result;
                foreach (var iface in source.GetInterfaces())
                {
                    if (_mappers.TryGetValue(TypePair.Create(iface, types.Target), out result))
                        return result;
                }

                if (_mappers.TryGetValue(TypePair.Create(source, types.Target), out result))
                    return result;
            }
            while ((source = source.BaseType) != null);

            return null;
        }
    }
}
