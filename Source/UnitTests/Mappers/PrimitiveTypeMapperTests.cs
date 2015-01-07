using System;
using System.Diagnostics;
using TinyMapper.Mappers;
using Xunit;

namespace UnitTests.Mappers
{
    public sealed class PrimitiveTypeMapperTests
    {
        [Fact]
        public void Map_Enum_Success()
        {

            var mapper = new PrimitiveTypeMapper();
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 100000; i++)
            {
                B.Enum value = mapper.Map<A.Enum, B.Enum>(A.Enum.Item1);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);

            //            Assert.Equal(B.Enum.Item1, value);
        }


        private static class A
        {
            public enum Enum
            {
                Item1,
                Item2,
                Item3
            }
        }


        private static class B
        {
            public enum Enum
            {
                Item1,
                Item2,
                Item3
            }
        }
    }
}
