using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using TinyMapper.Nelibur.Sword.DataStructures;
using TinyMapper.Nelibur.Sword.Extensions;

namespace TinyMapper.TypeConverters
{
    internal static class PrimitiveTypeConverter
    {
        private static readonly List<Func<Type, Type, Option<Func<object, object>>>> _converters = new List<Func<Type, Type, Option<Func<object, object>>>>();

        static PrimitiveTypeConverter()
        {
            _converters.Add(GetConversionMethod);
        }

        public static TTarget Convert<TSource, TTarget>(TSource value)
        {
            if (value.IsNull())
            {
                return default(TTarget);
            }
            Option<Func<object, object>> converter = GetConverter<TSource, TTarget>();
            if (converter.HasValue)
            {
                var result = (TTarget)converter.Value(value);
                return result;
            }
            return default(TTarget);
        }

        public static MethodInfo GetConverter(Type sourceType, Type targetType)
        {
            return typeof(PrimitiveTypeConverter).GetMethod("Convert", BindingFlags.Static | BindingFlags.Public)
                                                 .MakeGenericMethod(sourceType, targetType);
        }

        private static Option<Func<object, object>> GetConversionMethod(Type source, Type target)
        {
            if (source == target)
            {
                Func<object, object> result = x => x;
                return result.ToOption();
            }

            TypeConverter fromConverter = TypeDescriptor.GetConverter(source);
            if (fromConverter.CanConvertTo(target))
            {
                Func<object, object> result = x => fromConverter.ConvertTo(x, target);
                return result.ToOption();
            }

            TypeConverter toConverter = TypeDescriptor.GetConverter(target);
            if (toConverter.CanConvertFrom(source))
            {
                Func<object, object> result = x => toConverter.ConvertFrom(x);
                return result.ToOption();
            }

            if (IsEnumToEnumConversion(source, target))
            {
                Func<object, object> result = x => System.Convert.ChangeType(x, source);
                return result.ToOption();
            }
            return Option<Func<object, object>>.Empty;
        }

        private static Option<Func<object, object>> GetConverter<TFrom, TTo>()
        {
            foreach (Func<Type, Type, Option<Func<object, object>>> converter in _converters)
            {
                Option<Func<object, object>> func = converter(typeof(TFrom), typeof(TTo));
                if (func.HasValue)
                {
                    return func;
                }
            }
            return Option<Func<object, object>>.Empty;
        }

        private static bool IsEnumToEnumConversion(Type source, Type target)
        {
            return source.IsEnum && target.IsEnum;
        }
    }
}
