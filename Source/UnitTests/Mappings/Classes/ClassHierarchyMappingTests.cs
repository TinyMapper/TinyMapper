using System;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests.Mappings.Classes
{
    public sealed class ClassHierarchyMappingTests
    {
        [Fact]
        public void Map_Hierarchy_Success()
        {
            TinyMapper.Bind<Source, Target>();

            var source = new Source
            {
                Id = 1,
                String = "tiny"
            };

            var actual = TinyMapper.Map<Target>(source);

            Assert.Equal(source.Id, actual.Id);
            Assert.Equal(source.String, actual.String);
        }


        public class Source : SourceBase
        {
            public string String { get; set; }
        }


        public abstract class SourceBase
        {
            public int Id { get; set; }
        }


        public abstract class TargetBase
        {
            public int Id { get; set; }
        }


        public sealed class Target : TargetBase
        {
            public string String { get; set; }
        }
    }
}
