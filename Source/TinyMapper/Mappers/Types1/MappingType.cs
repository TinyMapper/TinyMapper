using System.Collections.Generic;
using TinyMappers.DataStructures;
using TinyMappers.Mappers.Types1.Members;

namespace TinyMappers.Mappers.Types1
{
    internal sealed class MappingType
    {
        public MappingType(TypePair typePair)
        {
            Members = new List<MappingMember>();
            TypePair = typePair;
        }

        public List<MappingMember> Members { get; private set; }
        public TypePair TypePair { get; private set; }
    }
}
