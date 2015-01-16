using System;
using System.ComponentModel;
using System.Reflection;
using TinyMapper.DataStructures;
using TinyMapper.Nelibur.Sword.Extensions;

namespace TinyMapper.TypeConverters
{
    internal static class PrimitiveTypeConverter
    {
        public static TTarget Convert<TSource, TTarget>(TSource value)
        {
            if (value.IsNull())
            {
                return default(TTarget);
            }
            var typePair = new TypePair(typeof(TSource), typeof(TTarget));
            Func<object, object> converter = GetConverter(typePair);
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

        private static Func<object, object> GetConverter(TypePair pair)
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

        private static bool IsEnumToEnumConversion(Type source, Type target)
        {
            return source.IsEnum && target.IsEnum;
        }

        private static object ReturnSameValue(object value)
        {
            return value;
        }
    }
}
