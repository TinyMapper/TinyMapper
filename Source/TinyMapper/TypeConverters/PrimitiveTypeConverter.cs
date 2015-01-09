using System;
using System.Collections.Generic;
using System.ComponentModel;
using TinyMapper.Nelibur.Sword.DataStructures;
using TinyMapper.Nelibur.Sword.Extensions;

namespace TinyMapper.TypeConverters
{
    internal class PrimitiveTypeConverter
    {
        private readonly List<Func<Type, Type, Option<Func<object, object>>>> _converters = new List<Func<Type, Type, Option<Func<object, object>>>>();

        public PrimitiveTypeConverter()
        {
            _converters.Add(GetConversionMethod);
        }

        public TTarget Convert<TSource, TTarget>(TSource value)
        {
            if (value.IsNull())
            {
                return default(TTarget);
            }
            Option<Func<object, object>> converter = GetConverter<TSource, TTarget>();
            if (converter.HasValue)
            {
                return (TTarget)converter.Value(value);
            }
            return default(TTarget);
        }

        private static Option<Func<object, object>> GetConversionMethod(Type source, Type target)
        {
            if (source == null || target == null)
            {
                return Option<Func<object, object>>.Empty;
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

        private static bool IsEnumToEnumConversion(Type source, Type target)
        {
            return source.IsEnum && target.IsEnum;
        }

        private Option<Func<object, object>> GetConverter<TFrom, TTo>()
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
    }
}
