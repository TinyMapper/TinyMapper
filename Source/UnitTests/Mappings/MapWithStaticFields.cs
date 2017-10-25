using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests.Mappings
{
    public class MapWithStaticFields
    {
        [Fact]
        public void MapStaticFields()
        {
            var source = new SourceStatic();

            TinyMapper.Bind<SourceStatic, TargetDto>();
            var actual = TinyMapper.Map<TargetDto>(source);

            Assert.Equal(SourceStatic.Id, actual.Id);
            Assert.Equal(SourceStatic.Name, actual.Name);
        }
    }

    public class SourceStatic
    {
        public static string Name = "test";
        public static int Id = 1;
    }


    public class TargetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
