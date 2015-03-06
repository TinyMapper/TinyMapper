using System;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
{
    public sealed class ClassHierarchyMappingTests
    {
        [Fact]
        public void Map_Hierarchy_Success()
        {
            TinyMapper.Bind<Class11, Class12>();

            var source = new Class11
            {
                Id = 1,
                String = "tiny"
            };

            var actual = TinyMapper.Map<Class12>(source);

            Assert.Equal(source.Id, actual.Id);
            Assert.Equal(source.String, actual.String);
        }
    }


    public class Class11 : ClassBase1
    {
        public string String { get; set; }
    }


    public abstract class ClassBase1
    {
        public int Id { get; set; }
    }


    public abstract class ClassBase2
    {
        public int Id { get; set; }
    }


    public sealed class Class12 : ClassBase2
    {
        public string String { get; set; }
    }
}
