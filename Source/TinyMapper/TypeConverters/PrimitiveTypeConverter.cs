using System;
using System.ComponentModel;
using System.Reflection;
using TinyMapper.DataStructures;

namespace TinyMapper.TypeConverters
{
    internal static class PrimitiveTypeConverter
    {
        public static TTarget ConvertEnumToEnum<TTarget, TSource>(TSource value)
        {
            return (TTarget)Convert.ChangeType(value, typeof(TTarget));
        }

        public static TTarget ConvertFrom<TSource, TTarget>(TSource value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(TTarget));
            return (TTarget)converter.ConvertFrom(value);
        }

        public static TSource ConvertNothing<TSource>(TSource value)
        {
            return value;
        }

        public static TTarget ConvertTo<TSource, TTarget>(TSource value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(TSource));
            return (TTarget)converter.ConvertTo(value, typeof(TTarget));
        }

        public static MethodInfo GetConverter(TypePair typePair)
        {
            MethodInfo result = GetConverterImpl(typePair);
            return result;
        }

        private static MethodInfo GetConverterImpl(TypePair pair)
        {
            if (pair.Source == pair.Target && IsTypePrimitive(pair.Source))
            {
                return typeof(PrimitiveTypeConverter).GetMethod("ConvertNothing", BindingFlags.Static | BindingFlags.Public)
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

        private static bool IsTypePrimitive(Type type)
        {
            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(string)
                   || type == typeof(Guid)
                   || type == typeof(decimal);
        }
    }
}
