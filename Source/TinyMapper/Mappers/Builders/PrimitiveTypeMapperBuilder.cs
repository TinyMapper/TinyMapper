using System;
using System.ComponentModel;
using TinyMapper.DataStructures;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers.Builders
{
    internal sealed class PrimitiveTypeMapperBuilder : MapperBuilder
    {
        public PrimitiveTypeMapperBuilder(IDynamicAssembly dynamicAssembly, TargetMapperBuilder targetMapperBuilder)
            : base(dynamicAssembly, targetMapperBuilder)
        {
        }

        public override bool IsSupported(TypePair typePair)
        {
            return IsTypePrimitive(typePair.Target)
                   || HasTypeConverter(typePair);
        }

        protected override Mapper CreateCore(TypePair typePair)
        {
            throw new NotImplementedException();
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
