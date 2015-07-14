using System;
using System.ComponentModel;
using Nelibur.ObjectMapper.Core.DataStructures;

namespace Nelibur.ObjectMapper.Mappers.Types.Convertible
{
    internal sealed class ConvertibleTypeMapperBuilder : MapperBuilder
    {
        private static readonly Func<object, object> _nothingConverter = x => x;

        public ConvertibleTypeMapperBuilder(IMapperBuilderConfig config) : base(config)
        {
        }

        protected override string ScopeName
        {
            get { return "ConvertibleTypeMappers"; }
        }

        protected override Mapper BuildCore(TypePair typePair)
        {
            Func<object, object> converter = GetConverter(typePair);
            return new ConvertibleTypeMapper(converter);
        }

        protected override bool IsSupportedCore(TypePair typePair)
        {
            return typePair.Source.IsPrimitive
                   || typePair.Source == typeof(string)
                   || typePair.Source == typeof(Guid)
                   || typePair.Source.IsEnum
                   || typePair.Source == typeof(decimal)
                   || typePair.HasTypeConverter();
        }

        private static Func<object, object> GetConverter(TypePair pair)
        {
            if (pair.IsDeepCloneable)
            {
                return _nothingConverter;
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

            if (pair.IsEnumTypes)
            {
                return x => Convert.ChangeType(x, pair.Source);
            }
            
            var conversionMethod = pair.Target.GetMethods(conversionFlags)
                .Where(m => m.Name == "op_Implicit"
                    && m.Attributes.HasFlag(MethodAttributes.SpecialName)
                    && m.GetParameters().Length == 1
                    && m.GetParameters()[0].ParameterType.IsAssignableFrom(pair.Source)
                ).FirstOrDefault();

            if (conversionMethod != null)
            {
                return x => conversionMethod.Invoke(null, new[] { x });
            }
            
            return null;
        }
    }
}
