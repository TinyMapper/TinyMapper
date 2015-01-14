using TinyMapper.TypeConverters;
using Xunit;
using Xunit.Extensions;

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
            Enum2 value = PrimitiveTypeConverter.Convert<Enum1, Enum2>(Enum1.Item1);
            Assert.Equal(Enum2.Item1, value);
        }

//        [Theory]
//        [InlineData(1, 1)]
//        [InlineData(null, 0)]
//        public void Map_NullableToInt_Success(int? sourceValue, int expected)
//        {
//            int value = PrimitiveTypeConverter.Convert<int?, int>(sourceValue);
//            Assert.Equal(expected, value);
//        }

        [Fact]
        public void Map_StringToEnum_Success()
        {
            Enum2 value = PrimitiveTypeConverter.Convert<string, Enum2>(Enum1.Item1.ToString());
            Assert.Equal(Enum2.Item1, value);
        }

        [Fact]
        public void Map_StringToInt_Success()
        {
            int value = PrimitiveTypeConverter.Convert<string, int>("2");
            Assert.Equal(2, value);
        }


        public class MyClass
        {
            public int Id;
        }
    }
}
