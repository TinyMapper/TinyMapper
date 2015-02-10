using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers.MappingMembers;
using Xunit;

namespace UnitTests.Mappers.Types
{
    public sealed class MappingTypeBuilderTests
    {
        [Fact]
        public void Buid_Recursion_Ok()
        {
            List<MappingMember> members = MappingMemberBuilder.Build(new TypePair(typeof(MyClass), typeof(MyClass1)));
            Assert.Equal(2, members.Count);
        }
    }


    public class MyClass
    {
        public MyClass1 Class { get; set; }
        public int Id { get; set; }
    }


    public class MyClass1
    {
        public MyClass Class { get; set; }
        public int Id { get; set; }
    }
}
