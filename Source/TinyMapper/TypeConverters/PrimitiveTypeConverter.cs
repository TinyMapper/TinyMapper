using System;
using System.ComponentModel;
using System.Reflection;
using Nelibur.ObjectMapper.Core.DataStructures;

namespace Nelibur.ObjectMapper.TypeConverters
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

        public static bool IsSupported(TypePair typePair)
        {
            return IsTypePrimitive(typePair.Target)
                   || HasTypeConverter(typePair);
        }

        private static MethodInfo GetConverterImpl(TypePair typePair)
        {
            if (typePair.IsEqualTypes && IsTypePrimitive(typePair.Source))
            {
                return typeof(PrimitiveTypeConverter).GetMethod("ConvertNothing", BindingFlags.Static | BindingFlags.Public)
                                                     .MakeGenericMethod(typePair.Source);
            }

            TypeConverter fromConverter = TypeDescriptor.GetConverter(typePair.Source);
            if (fromConverter.CanConvertTo(typePair.Target))
            {
                return typeof(PrimitiveTypeConverter).GetMethod("ConvertTo", BindingFlags.Static | BindingFlags.Public)
                                                     .MakeGenericMethod(typePair.Source, typePair.Target);
            }

            TypeConverter toConverter = TypeDescriptor.GetConverter(typePair.Target);
            if (toConverter.CanConvertFrom(typePair.Source))
            {
                return typeof(PrimitiveTypeConverter).GetMethod("ConvertFrom", BindingFlags.Static | BindingFlags.Public)
                                                     .MakeGenericMethod(typePair.Source, typePair.Target);
            }

            if (typePair.IsEnumTypes)
            {
                return typeof(PrimitiveTypeConverter).GetMethod("ConvertEnumToEnum", BindingFlags.Static | BindingFlags.Public)
                                                     .MakeGenericMethod(typePair.Source, typePair.Target);
            }
            return null;
        }

        private static bool HasTypeConverter(TypePair pair)
        {
            TypeConverter fromConverter = TypeDescriptor.GetConverter(pair.Source);
            if (fromConverter.CanConvertTo(pair.Target))
            {
                return true;
            }

            TypeConverter toConverter = TypeDescriptor.GetConverter(pair.Target);
            if (toConverter.CanConvertFrom(pair.Source))
            {
                return true;
            }
            return false;
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
