using System;
using System.Collections;
using TinyMapper.DataStructures;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers.Builders
{
    internal sealed class CollectionMapperBuilder : MapperBuilder
    {
        public CollectionMapperBuilder(IDynamicAssembly dynamicAssembly, TargetMapperBuilder targetMapperBuilder)
            : base(dynamicAssembly, targetMapperBuilder)
        {
        }

        public override bool IsSupported(TypePair typePair)
        {
            return typePair.Target.IsArray
                   || typeof(IEnumerable).IsAssignableFrom(typePair.Target);
        }

        protected override Mapper CreateCore(TypePair typePair)
        {
            throw new NotImplementedException();
        }
    }
}
