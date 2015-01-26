using System;
using System.Collections.Generic;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Types1.Members;

namespace TinyMapper.Mappers.Types1
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
