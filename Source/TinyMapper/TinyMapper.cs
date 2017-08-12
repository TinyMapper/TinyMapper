using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
#if COREFX
using System.Reflection;
#endif
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
    /// TinyMapper is an object to object mapper for .NET. The main advantage is performance.
    /// TinyMapper allows easily map object to object, i.e. properties or fields from one object to another.
    /// </summary>
    public static class TinyMapper
    {
        private static readonly Dictionary<TypePair, Mapper> _mappers = new Dictionary<TypePair, Mapper>();
        private static readonly TargetMapperBuilder _targetMapperBuilder;
        private static readonly TinyMapperConfig _config;
        private static readonly object _mappersLock = new object();

        static TinyMapper()
        {
            IDynamicAssembly assembly = DynamicAssemblyBuilder.Get();
            _targetMapperBuilder = new TargetMapperBuilder(assembly);
            _config = new TinyMapperConfig(_targetMapperBuilder);
        }

        /// <summary>
        /// Create a one-way mapping between <see cref="TSource"/> <see cref="TTarget"/>.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TTarget">Target type.</typeparam>
        /// <remarks>The method is thread safe.</remarks>
        public static void Bind<TSource, TTarget>()
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();
            lock (_mappersLock)
            {
                _mappers[typePair] = _targetMapperBuilder.Build(typePair);
            }
        }

        /// <summary>
        /// Create a one-way mapping between <see cref="TSource"/> <see cref="TTarget"/>.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TTarget">Target type.</typeparam>
        /// <param name="config">BindingConfig for custom binding.</param>
        /// <remarks>The method is thread safe.</remarks>
        public static void Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config)
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();

            var bindingConfig = new BindingConfigOf<TSource, TTarget>();
            config(bindingConfig);

            lock (_mappersLock)
            {
                _mappers[typePair] = _targetMapperBuilder.Build(typePair, bindingConfig);
            }
        }

        /// <summary>
        /// Find out if a binding exists from <see cref="TSource"/> to <see cref="TTarget"/>.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TTarget">Target type.</typeparam>
        /// <returns>True if exists, otherwise - False.</returns>
        /// <remarks>The method is thread safe.</remarks>
        public static bool BindingExists<TSource, TTarget>()
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();
            lock (_mappersLock)
            {
                return _mappers.ContainsKey(typePair);
            }
        }

        /// <summary>
        /// Mapsthe source to <see cref="TTarget"/> type.
        /// The method can be called in parallel to Map methods, but cannot be called in parallel to Bind method.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TTarget">Target type.</typeparam>
        /// <param name="source">Source object.</param>
        /// <param name="target">Target object.</param>
        /// <returns>Mapped object.</returns>
        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target = default(TTarget))
        {
            TypePair typePair = TypePair.Create<TSource, TTarget>();

            Mapper mapper = GetMapper(typePair);
            var result = (TTarget)mapper.Map(source, target);

            return result;
        }

        /// <summary>
        /// Configure the Mapper.
        /// </summary>
        /// <param name="config">Lambda to provide config settings</param>
        public static void Config(Action<ITinyMapperConfig> config)
        {
            config(_config);
        }

        /// <summary>
        /// Maps the source to <see cref="TTarget"/> type.
        /// The method can be called in parallel to Map methods, but cannot be called in parallel to Bind method.
        /// </summary>
        /// <typeparam name="TTarget">Target type.</typeparam>
        /// <param name="source">Source object [Not null].</param>
        /// <returns>Mapped object. The method can be called in parallel to Map methods, but cannot be called in parallel to Bind method.</returns>
        public static TTarget Map<TTarget>(object source)
        {
            if (source.IsNull())
            {
                throw Error.ArgumentNull("Source cannot be null. Use TinyMapper.Map<TSource, TTarget> method instead.");
            }

            TypePair typePair = TypePair.Create(source.GetType(), typeof(TTarget));

            Mapper mapper = GetMapper(typePair);
            var result = (TTarget)mapper.Map(source);

            return result;
        }

        [SuppressMessage("ReSharper", "All")]
        private static Mapper GetMapper(TypePair typePair)
        {
            Mapper mapper;

            if (_mappers.TryGetValue(typePair, out mapper) == false)
            {
                throw new TinyMapperException($"No binding found for '{typePair.Source.Name}' to '{typePair.Target.Name}'. " +
                                              $"Call TinyMapper.Bind<{typePair.Source.Name}, {typePair.Target.Name}>()");
            }

//            _mappersLock.EnterUpgradeableReadLock();
//            try
//            {
//                if (_mappers.TryGetValue(typePair, out mapper) == false)
//                {
//                    if (_config.EnablePolymorphicMapping && (mapper = GetPolymorphicMapping(typePair)) != null)
//                    {
//                        return mapper;
//                    }
//                    else if (_config.EnableAutoBinding)
//                    {
//                        mapper = _targetMapperBuilder.Build(typePair);
//                        _mappersLock.EnterWriteLock();
//                        try
//                        {
//                            _mappers[typePair] = mapper;
//                        }
//                        finally
//                        {
//                            _mappersLock.ExitWriteLock();
//                        }
//                    }
//                    else
//                    {
//                        throw new TinyMapperException($"Unable to find a binding for type '{typePair.Source?.Name}' to '{typePair.Target?.Name}'.");
//                    }
//                }
//            }
//            finally
//            {
//                _mappersLock.ExitUpgradeableReadLock();
//            }

            return mapper;
        }

        //Note: Lock should already be acquired for the mapper
//        private static Mapper GetPolymorphicMapping(TypePair types)
//        {
//            // Walk the polymorphic heirarchy until we find a mapping match
//            Type source = types.Source;
//
//            do
//            {
//                Mapper result;
//                foreach (Type iface in source.GetInterfaces())
//                {
//                    if (_mappers.TryGetValue(TypePair.Create(iface, types.Target), out result))
//                        return result;
//                }
//
//                if (_mappers.TryGetValue(TypePair.Create(source, types.Target), out result))
//                    return result;
//            }
//            while ((source = Helpers.BaseType(source)) != null);
//
//            return null;
//        }
    }
}
