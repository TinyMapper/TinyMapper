using System;
using System.ComponentModel;
using Nelibur.ObjectMapper.Core.DataStructures;

namespace Nelibur.ObjectMapper.Mappers.Types.Custom
{
    internal sealed class CustomTypeMapperBuilder : MapperBuilder
    {
        public CustomTypeMapperBuilder(IMapperBuilderConfig config) : base(config)
        {
        }

        protected override string ScopeName
        {
            get { return "CustomTypeMappers"; }
        }

        protected override Mapper BuildCore(TypePair typePair)
        {
            Func<object, object> converter = GetConverter(typePair);
            return new CustomTypeMapper(converter);
        }

        protected override bool IsSupportedCore(TypePair typePair)
        {
            return typePair.HasTinyMapperConverter();
        }

        private static Func<object, object> GetConverter(TypePair pair)
        {
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

            if (pair.IsEnumTypes)
            {
                return x => Convert.ChangeType(x, pair.Source);
            }
            return null;
        }
    }
}
