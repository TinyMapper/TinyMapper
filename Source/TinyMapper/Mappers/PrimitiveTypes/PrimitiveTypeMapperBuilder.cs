using System;
using Nelibur.ObjectMapper.Core.DataStructures;

namespace Nelibur.ObjectMapper.Mappers.PrimitiveTypes
{
    internal sealed class PrimitiveTypeMapperBuilder : MapperBuilder
    {
        public PrimitiveTypeMapperBuilder(IMapperBuilderConfig config) : base(config)
        {
        }

        protected override string ScopeName
        {
            get { return "PrimitiveTypeMappers"; }
        }

        protected override Mapper CreateCore(TypePair typePair)
        {
            throw new NotImplementedException();
        }

        protected override bool IsSupportedCore(TypePair typePair)
        {
            return typePair.Source.IsPrimitive
                   || typePair.Source == typeof(string)
                   || typePair.Source == typeof(Guid)
                   || typePair.Source.IsEnum
                   || typePair.Source == typeof(decimal);
        }
    }
}
