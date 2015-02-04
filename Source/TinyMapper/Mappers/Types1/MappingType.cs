using System.Collections.Generic;
using Nelibur.Mapper.Core.DataStructures;
using Nelibur.Mapper.Mappers.Types1.Members;

namespace Nelibur.Mapper.Mappers.Types1
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
