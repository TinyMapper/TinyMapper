using System;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers;
using Nelibur.ObjectMapper.Mappers.Types;
using Xunit;
using Xunit.Extensions;

namespace UnitTests.Mappers.Types
{
    public sealed class PrimitiveTypeMapperTests
    {
        [Fact]
        public void Map_Enum_Success()
        {
            var builder = new PrimitiveTypeMapperBuilder(new MappingBuilderConfigStub());
            Mapper mapper = builder.Build(new TypePair(typeof(EnumA), typeof(EnumB)));

            var actual = (EnumB)mapper.Map(EnumA.B);
            Assert.Equal(EnumB.B, actual);
        }

        [InlineData(typeof(bool), typeof(bool), true, true)]
        [InlineData(typeof(byte), typeof(byte), 0, 0)]
        [InlineData(typeof(int), typeof(int), 1, 1)]
        [InlineData(typeof(string), typeof(int), "1", 1)]
        [InlineData(typeof(int), typeof(string), 1, "1")]
        [InlineData(typeof(decimal), typeof(decimal), 5, 5)]
        [InlineData(typeof(float), typeof(float), 6, 6)]
        [InlineData(typeof(long), typeof(long), 7, 7)]
        [InlineData(typeof(ulong), typeof(ulong), 2, 2)]
        [InlineData(typeof(sbyte), typeof(sbyte), 8, 8)]
        [InlineData(typeof(short), typeof(short), 10, 10)]
        [InlineData(typeof(ushort), typeof(ushort), 10, 10)]
        [InlineData(typeof(char), typeof(char), 'a', 'a')]
        [InlineData(typeof(string), typeof(string), "abc", "abc")]
        [Theory]
        public void Map_PrimitiveTypes_Success(Type sourceType, Type targetType, object source, object expected)
        {
            var builder = new PrimitiveTypeMapperBuilder(new MappingBuilderConfigStub());
            Mapper mapper = builder.Build(new TypePair(sourceType, targetType));
            object actual = mapper.Map(source);
            Assert.Equal(expected, actual);
        }
    }


    public enum EnumA
    {
        A,
        B,
        C
    }


    public enum EnumB
    {
        A,
        B,
        C
    }
}
