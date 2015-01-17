using System;
using System.ComponentModel;
using TinyMapper.DataStructures;

namespace TinyMapper.Mappers
{
    internal sealed class PrimitiveTypeMapper : IMapper
    {
        public bool IsSupported(TypePair typePair)
        {
            return IsTypePrimitive(typePair.Target)
                   || HasTypeConverter(typePair);
        }

        private static bool IsTypePrimitive(Type type)
        {
            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(string)
                   || type == typeof(Guid)
                   || type == typeof(decimal);
        }

        private bool HasTypeConverter(TypePair pair)
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
    }
}
