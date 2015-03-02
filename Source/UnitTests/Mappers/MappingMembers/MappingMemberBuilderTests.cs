using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers.Classes.Members;
using Xunit;

namespace UnitTests.Mappers.MappingMembers
{
    public sealed class MappingMemberBuilderTests
    {
        [Fact]
        public void Buid_Recursion_Success()
        {
            var mappingMemberBuilder = new MappingMemberBuilder(new MappingBuilderConfigStub());
            List<MappingMember> members = mappingMemberBuilder.Build(new TypePair(typeof(MyClass), typeof(MyClass1)));
            Assert.Equal(2, members.Count);
        }

        [Fact]
        public void Build_CommonFileds_Success()
        {
            var mappingMemberBuilder = new MappingMemberBuilder(new MappingBuilderConfigStub());
            List<MappingMember> members = mappingMemberBuilder.Build(new TypePair(typeof(MyClass2), typeof(MyClass3)));
            Assert.Equal(2, members.Count);
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


        public class MyClass2
        {
            private string _private;
            public int Int { get; set; }
            public long Long { get; set; }
            public string String { get; set; }
        }


        public class MyClass3
        {
            private string _private;
            public int Int { get; set; }
            public long Long { get; set; }
            public string String1 { get; set; }
        }
    }
}
