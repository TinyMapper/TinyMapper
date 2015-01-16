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

        public static TTarget ConvertEnumToEnum<TTarget, TSource>(TSource value)
        {
            return (TTarget)System.Convert.ChangeType(value, typeof(TTarget));
        }

        public static TTarget ConvertFrom<TSource, TTarget>(TSource value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(TTarget));
            return (TTarget)converter.ConvertFrom(value);
        }

        public static TSource ConvertSame<TSource>(TSource value)
        {
            return value;
        }

        public static TTarget ConvertTo<TSource, TTarget>(TSource value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(TSource));
            return (TTarget)converter.ConvertTo(value, typeof(TTarget));
        }

        public static MethodInfo GetConverter(Type sourceType, Type targetType)
        {
            return GetConverter1(new TypePair(sourceType, targetType));
            //            return typeof(PrimitiveTypeConverter).GetMethod("Convert", BindingFlags.Static | BindingFlags.Public)
            //                                                 .MakeGenericMethod(sourceType, targetType);
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

        private static MethodInfo GetConverter1(TypePair pair)
        {
            if (pair.Source == pair.Target)
            {
                return typeof(PrimitiveTypeConverter).GetMethod("ConvertSame", BindingFlags.Static | BindingFlags.Public)
                                                     .MakeGenericMethod(pair.Source);
            }

            TypeConverter fromConverter = TypeDescriptor.GetConverter(pair.Source);
            if (fromConverter.CanConvertTo(pair.Target))
            {
                return typeof(PrimitiveTypeConverter).GetMethod("ConvertTo", BindingFlags.Static | BindingFlags.Public)
                                                     .MakeGenericMethod(pair.Source, pair.Target);
            }

            TypeConverter toConverter = TypeDescriptor.GetConverter(pair.Target);
            if (toConverter.CanConvertFrom(pair.Source))
            {
                return typeof(PrimitiveTypeConverter).GetMethod("ConvertFrom", BindingFlags.Static | BindingFlags.Public)
                                                     .MakeGenericMethod(pair.Source, pair.Target);
            }

            if (IsEnumToEnumConversion(pair.Source, pair.Target))
            {
                return typeof(PrimitiveTypeConverter).GetMethod("ConvertEnumToEnum", BindingFlags.Static | BindingFlags.Public)
                                                     .MakeGenericMethod(pair.Source, pair.Target);
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
