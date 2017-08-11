using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper.Bindings;
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
            List<MappingMember> members = mappingMemberBuilder.Build(new TypePair(typeof(Source1), typeof(Target1)));
            Assert.Equal(2, members.Count);
        }

        [Fact]
        public void Build_CommonFileds_Success()
        {
            var mappingMemberBuilder = new MappingMemberBuilder(new MappingBuilderConfigStub());
            List<MappingMember> members = mappingMemberBuilder.Build(new TypePair(typeof(Source2), typeof(Target2)));
            Assert.Equal(2, members.Count);
        }

        [Fact]
        public void Build_IgnoreProperty_Success()
        {
            var bindingConfig = new BindingConfig();
            bindingConfig.IgnoreSourceField("Id");

            var mappingMemberBuilder = new MappingMemberBuilder(new MappingBuilderConfigStub(bindingConfig));

            List<MappingMember> members = mappingMemberBuilder.Build(new TypePair(typeof(Source1), typeof(Target1)));
            Assert.Equal(1, members.Count);
        }


        public class Source1
        {
            public Target1 Class { get; set; }
            public int Id { get; set; }
        }


        public class Target1
        {
            public Source1 Class { get; set; }
            public int Id { get; set; }
        }


        public class Source2
        {
            public int Int { get; set; }
            public long Long { get; set; }
            public string String { get; set; }
        }


        public class Target2
        {
            public int Int { get; set; }
            public long Long { get; set; }
            public string String1 { get; set; }
        }
    }
}
