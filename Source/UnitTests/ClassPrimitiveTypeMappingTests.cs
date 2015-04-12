using System;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Reflection;
using Xunit;

namespace UnitTests
{
    public sealed class ClassPrimitiveTypeMappingTests
    {
        [Fact]
        public void Map_PrimitiveType_Success()
        {
            TinyMapper.Bind<Source, Target>();
            DynamicAssemblyBuilder.Get().Save();

            var source = new Source();

            var actual = TinyMapper.Map<Target>(source);

            Assert.Equal(source.Bool, actual.Bool);
            Assert.Equal(source.Byte, actual.Byte);
            Assert.Equal(source.Char, actual.Char);
            Assert.Equal(source.Decimal, actual.Decimal);
            Assert.Equal(source.Float, actual.Float);
            Assert.Equal(source.Int, actual.Int);
            Assert.Equal(source.Int1, actual.Int1);
            Assert.Equal(source.Int2, actual.Int2);
            Assert.Equal(source.Int3.GetValueOrDefault(), actual.Int3);
            Assert.Equal(source.Int4.GetValueOrDefault(), actual.Int4);
            Assert.Equal(source.Long, actual.Long);
            Assert.Equal(source.Sbyte, actual.Sbyte);
            Assert.Equal(source.Short, actual.Short);
            Assert.Equal(source.String, actual.String);
            Assert.Equal(source.Ulong, actual.Ulong);
            Assert.Equal(source.Ushort, actual.Ushort);
            Assert.Equal(source.DateTime, actual.DateTime);
            Assert.Equal(source.DateTimeOffset, actual.DateTimeOffset);
            Assert.Equal(source.DateTimeNullable, actual.DateTimeNullable);
            Assert.Equal(source.DateTimeNullable1, actual.DateTimeNullable1);
        }


        public class Source
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public char Char { get; set; }
            public DateTime DateTime { get; set; }
            public DateTime? DateTimeNullable { get; set; }
            public DateTime? DateTimeNullable1 { get; set; }
            public DateTimeOffset DateTimeOffset { get; set; }
            public decimal Decimal { get; set; }
            public float Float { get; set; }
            public int Int { get; set; }
            public int? Int1 { get; set; }
            public int Int2 { get; set; }
            public int? Int3 { get; set; }
            public int? Int4 { get; set; }
            public long Long { get; set; }
            public sbyte Sbyte { get; set; }
            public short Short { get; set; }
            public string String { get; set; }
            public ulong Ulong { get; set; }
            public ushort Ushort { get; set; }
        }


        public class Target
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public char Char { get; set; }
            public DateTime DateTime { get; set; }
            public DateTime? DateTimeNullable { get; set; }
            public DateTime? DateTimeNullable1 { get; set; }
            public DateTimeOffset DateTimeOffset { get; set; }
            public decimal Decimal { get; set; }
            public float Float { get; set; }
            public int Int { get; set; }
            public int? Int1 { get; set; }
            public int? Int2 { get; set; }
            public int Int3 { get; set; }
            public int Int4 { get; set; }
            public long Long { get; set; }
            public sbyte Sbyte { get; set; }
            public short Short { get; set; }
            public string String { get; set; }
            public ulong Ulong { get; set; }
            public ushort Ushort { get; set; }
        }
    }
}
