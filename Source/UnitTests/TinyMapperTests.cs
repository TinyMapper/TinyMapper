using System;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
{
    public sealed class TinyMapperTests
    {
        [Fact]
        public void Map_ClassWithPrimitiveTypes_Success()
        {
            TinyMapper.Bind<Class1, Class2>();
            var source = new Class1
            {
                Bool = true,
                Byte = 1,
                Char = 'a',
                Decimal = 1,
                Double = 2,
                Float = 1.0f,
                Int = 1,
                Long = 2,
                Sbyte = 2,
                Short = 3,
                String = "test",
                Uint = 1,
                Ulong = 3,
                Ushort = 2
            };

            var target = TinyMapper.Map<Class2>(source);

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

        [Fact]
        public void Map_NullSource_ThrowException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => TinyMapper.Map<string>(null));
            Assert.Throws(typeof(ArgumentNullException), () => TinyMapper.Map<string, string>(null));
        }


        public class Class1
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public char Char { get; set; }
            public decimal Decimal { get; set; }
            public double Double { get; set; }
            public float Float { get; set; }
            public int Int { get; set; }
            public long Long { get; set; }
            public sbyte Sbyte { get; set; }
            public short Short { get; set; }
            public string String { get; set; }
            public uint Uint { get; set; }
            public ulong Ulong { get; set; }
            public ushort Ushort { get; set; }
        }


        public class Class2
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public char Char { get; set; }
            public decimal Decimal { get; set; }
            public double Double { get; set; }
            public float Float { get; set; }
            public int Int { get; set; }
            public long Long { get; set; }
            public sbyte Sbyte { get; set; }
            public short Short { get; set; }
            public string String { get; set; }
            public uint Uint { get; set; }
            public ulong Ulong { get; set; }
            public ushort Ushort { get; set; }
        }
    }
}
