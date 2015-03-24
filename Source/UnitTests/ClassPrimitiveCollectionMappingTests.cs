using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
{
    public sealed class ClassPrimitiveCollectionMappingTests
    {
        [Fact]
        public void Map_PrimitiveArrays_Success()
        {
            TinyMapper.Bind<Class5, Class6>();

            var source = new Class5
            {
                Ints = new[] { 0, 1 },
                Bools = new[] { true },
                Strings = new[] { "Nelibur", "TinyMapper" }
            };

            var target = TinyMapper.Map<Class6>(source);

            Assert.Equal(target.Ints, source.Ints);
            Assert.Equal(target.Bools, source.Bools);
            Assert.Equal(target.Strings, source.Strings);
        }

        [Fact]
        public void Map_PrimitiveCollections_Success()
        {
            TinyMapper.Bind<Class3, Class4>();
            var source = new Class3
            {
                Bool = new List<bool> { true, false },
                Byte = new List<byte> { 0, 1 },
                Char = new List<char> { 'a', 'b' },
                Decimal = new List<decimal> { 1, 2 },
                Double = new List<double> { 2, 3 },
                Float = new List<float> { 1, 5 },
                Int = new List<int> { 0, 4, 3 },
                Long = new List<long> { 90, 23 },
                Sbyte = new List<sbyte> { 1, 1 },
                Short = new List<short> { 10, 11 },
                String = new List<string> { "f", "q" },
                Uint = new List<uint> { 9, 9 },
                Ulong = new List<ulong> { 2, 1 },
                Ushort = new List<ushort> { 5, 5 }
            };

            var target = TinyMapper.Map<Class4>(source);

            Assert.Equal(target.Bool, source.Bool);
            Assert.Equal(target.Byte, source.Byte);
            Assert.Equal(target.Char, source.Char);
            Assert.Equal(target.Decimal, source.Decimal);
            Assert.Equal(target.Double, source.Double);
            Assert.Equal(target.Float, source.Float);
            Assert.Equal(target.Int, source.Int);
            Assert.Equal(target.Long, source.Long);
            Assert.Equal(target.Sbyte, source.Sbyte);
            Assert.Equal(target.Short, source.Short);
            Assert.Equal(target.String, source.String);
            Assert.Equal(target.Uint, source.Uint);
            Assert.Equal(target.Ulong, source.Ulong);
            Assert.Equal(target.Ushort, source.Ushort);
        }


        public class Class3
        {
            public List<bool> Bool { get; set; }
            public List<byte> Byte { get; set; }
            public List<char> Char { get; set; }
            public List<decimal> Decimal { get; set; }
            public List<double> Double { get; set; }
            public List<float> Float { get; set; }
            public List<int> Int { get; set; }
            public List<long> Long { get; set; }
            public List<sbyte> Sbyte { get; set; }
            public List<short> Short { get; set; }
            public List<string> String { get; set; }
            public List<uint> Uint { get; set; }
            public List<ulong> Ulong { get; set; }
            public List<ushort> Ushort { get; set; }
        }


        public class Class4
        {
            public List<bool> Bool { get; set; }
            public List<byte> Byte { get; set; }
            public List<char> Char { get; set; }
            public List<decimal> Decimal { get; set; }
            public List<double> Double { get; set; }
            public List<float> Float { get; set; }
            public List<int> Int { get; set; }
            public List<long> Long { get; set; }
            public List<sbyte> Sbyte { get; set; }
            public List<short> Short { get; set; }
            public List<string> String { get; set; }
            public List<uint> Uint { get; set; }
            public List<ulong> Ulong { get; set; }
            public List<ushort> Ushort { get; set; }
        }


        public class Class5
        {
            public bool[] Bools { get; set; }
            public int[] Ints { get; set; }
            public string[] Strings { get; set; }
        }


        public class Class6
        {
            public bool[] Bools { get; set; }
            public int[] Ints { get; set; }
            public string[] Strings { get; set; }
        }
    }
}
