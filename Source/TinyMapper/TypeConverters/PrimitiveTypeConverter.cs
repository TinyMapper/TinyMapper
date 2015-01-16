using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using TinyMapper.DataStructures;
using TinyMapper.Nelibur.Sword.DataStructures;
using TinyMapper.Nelibur.Sword.Extensions;

namespace TinyMapper.TypeConverters
{
    internal static class PrimitiveTypeConverter
    {
        private static readonly Dictionary<TypePair, Option<Func<object, object>>> _t = new Dictionary<TypePair, Option<Func<object, object>>>();

        public static TTarget Convert<TSource, TTarget>(TSource value)
        {
            if (value.IsNull())
            {
                return default(TTarget);
            }
            Func<object, object> converter = GetConverter<TSource, TTarget>();
            if (converter != null)
            {
                var result = (TTarget)converter(value);
                return result;
            }
            throw new NotSupportedException();
        }

        public static MethodInfo GetConverter(Type sourceType, Type targetType)
        {
            return typeof(PrimitiveTypeConverter).GetMethod("Convert", BindingFlags.Static | BindingFlags.Public)
                                                 .MakeGenericMethod(sourceType, targetType);
        }

        private static Func<object, object> GetConversionMethod(TypePair pair)
        {
            if (pair.Source == pair.Target)
            {
                return x => x;
            }

            TypeConverter fromConverter = TypeDescriptor.GetConverter(pair.Source);
            if (fromConverter.CanConvertTo(pair.Target))
            {
                return x => fromConverter.ConvertTo(x, pair.Target);
            }

            TypeConverter toConverter = TypeDescriptor.GetConverter(pair.Target);
            if (toConverter.CanConvertFrom(pair.Source))
            {
                return x => toConverter.ConvertFrom(x);
            }

            if (IsEnumToEnumConversion(pair.Source, pair.Target))
            {
                return x => System.Convert.ChangeType(x, pair.Source);
            }
            return null;
        }

        private static Func<object, object> GetConverter<TSource, TTarget>()
        {
            var typePair = new TypePair(typeof(TSource), typeof(TTarget));

            return GetConversionMethod(typePair);
        }

        private static bool IsEnumToEnumConversion(Type source, Type target)
        {
            return source.IsEnum && target.IsEnum;
        }
    }
}
