using TinyMapper.Mappers;
using Xunit;

namespace UnitTests.Mappers
{
    public sealed class PrimitiveTypeMapperTests
    {
        private enum Enum1
        {
            Item1,
            Item2,
            Item3
        }


        private enum Enum2
        {
            Item1,
            Item2,
            Item3
        }


        [Fact]
        public void Map_EnumToEnum_Success()
        {
            var mapper = new PrimitiveTypeMapper();
            Enum2 value = mapper.Map<Enum1, Enum2>(Enum1.Item1);
            Assert.Equal(Enum2.Item1, value);
        }

        [Fact]
        public void Map_StringToEnum_Success()
        {
            var mapper = new PrimitiveTypeMapper();
            Enum2 value = mapper.Map<string, Enum2>(Enum1.Item1.ToString());
            Assert.Equal(Enum2.Item1, value);
        }

        [Fact]
        public void Map_StringToInt_Success()
        {
            var mapper = new PrimitiveTypeMapper();
            int value = mapper.Map<string, int>("2");
            Assert.Equal(2, value);
        }
    }
}
