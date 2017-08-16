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
            List<MappingMemberPath> members = mappingMemberBuilder.Build(new TypePair(typeof(SourceWithRecursion), typeof(TargetWithRecursion)));
            Assert.Equal(2, members.Count);
        }

        [Fact]
        public void Build_CommonFileds_Success()
        {
            var mappingMemberBuilder = new MappingMemberBuilder(new MappingBuilderConfigStub());
            List<MappingMemberPath> members = mappingMemberBuilder.Build(new TypePair(typeof(SourceSimple), typeof(TargetSimple)));
            Assert.Equal(2, members.Count);
        }

        [Fact]
        public void Build_IgnoreProperty_Success()
        {
            var bindingConfig = new BindingConfig();
            bindingConfig.IgnoreSourceField(nameof(SourceWithRecursion.Id));

            var mappingMemberBuilder = new MappingMemberBuilder(new MappingBuilderConfigStub(bindingConfig));

            List<MappingMemberPath> members = mappingMemberBuilder.Build(new TypePair(typeof(SourceWithRecursion), typeof(TargetWithRecursion)));
            Assert.Equal(1, members.Count);
        }


        public class SourceWithRecursion
        {
            public TargetWithRecursion Class { get; set; }
            public int Id { get; set; }
        }


        public class TargetWithRecursion
        {
            public SourceWithRecursion Class { get; set; }
            public int Id { get; set; }
        }


        public class SourceSimple
        {
            public int Int { get; set; }
            public long Long { get; set; }
            public string String { get; set; }
        }


        public class TargetSimple
        {
            public int Int { get; set; }
            public long Long { get; set; }
            public string String1 { get; set; }
        }
    }
}
