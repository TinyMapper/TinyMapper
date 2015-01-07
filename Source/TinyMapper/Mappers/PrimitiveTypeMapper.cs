using System;
using System.Collections.Generic;
using System.ComponentModel;
using TinyMapper.Nelibur.Sword.DataStructures;
using TinyMapper.Nelibur.Sword.Extensions;

namespace TinyMapper.Mappers
{
    internal class PrimitiveTypeMapper
    {
        private readonly List<Func<Type, Type, Option<Func<object, object>>>> _converters = new List<Func<Type, Type, Option<Func<object, object>>>>();

        public PrimitiveTypeMapper()
        {
            _converters.Add(GetConversionMethod);
        }

        public TTo Map<TFrom, TTo>(TFrom value)
        {
            if (value.IsNull())
            {
                return default(TTo);
            }
            Option<Func<object, object>> converter = GetConverter<TFrom, TTo>();
            if (converter.HasValue)
            {
                return (TTo)converter.Value(value);
            }
            return default(TTo);
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
                Func<object, object> result = x => Convert.ChangeType(x, source);
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
