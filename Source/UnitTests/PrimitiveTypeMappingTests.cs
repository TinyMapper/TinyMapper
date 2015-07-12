using System;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
{
    public sealed class PrimitiveTypeMappingTests
    {
        [Fact]
        public void Map_PrimitiveType_Success()
        {
            TinyMapper.Bind<Source1, Target1>();

            var source = new Source1
            {
                Bool = true,
                Char = 'a',
                DateTime = DateTime.Now,
                IntNullable1 = 1,
                Decimal = 1m,
                DateTimeNullable1 = DateTime.Today
            };

            var actual = TinyMapper.Map<Target1>(source);

            Assert.Equal(source.Bool, actual.Bool);
            Assert.Equal(source.Byte, actual.Byte);
            Assert.Equal(source.Char, actual.Char);
            Assert.Equal(source.Decimal, actual.Decimal);
            Assert.Equal(source.Float, actual.Float);
            Assert.Equal(source.Int, actual.Int);
            Assert.Equal(source.Int1, actual.Int1);
            Assert.Equal(source.Int2, actual.Int2);
            Assert.Equal(source.IntNullable1.GetValueOrDefault(), actual.IntNullable1);
            Assert.Equal(source.IntNullable2.GetValueOrDefault(), actual.IntNullable2);
            Assert.Equal(source.Long, actual.Long);
            Assert.Equal(source.Sbyte, actual.Sbyte);
            Assert.Equal(source.Short, actual.Short);
            Assert.Equal(source.String, actual.String);
            Assert.Equal(source.Ulong, actual.Ulong);
            Assert.Equal(source.Ushort, actual.Ushort);
            Assert.Equal(source.DateTime, actual.DateTime);
            Assert.Equal(source.DateTimeOffset, actual.DateTimeOffset);
            Assert.Equal(source.DateTimeNullable1, actual.DateTimeNullable1);
            Assert.Equal(source.DateTimeNullable2, actual.DateTimeNullable2);
        }

        [Fact]
        public void Map_IgnoreProperties_Success()
        {
            TinyMapper.Bind<Source2, Target2>(config =>
            {
                config.Ignore(x => x.FirstName);
                config.Ignore(x => x.LastName);
            });

            var source = new Source2
            {
                Id = 1,
                FirstName = "First",
                LastName = "LastName"
            };

            var actual = TinyMapper.Map<Target2>(source);

            Assert.Equal(source.Id, actual.Id);
            Assert.Null(actual.FirstName);
            Assert.Null(actual.LastName);
        }


        public class Source2
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }


        public class Target2
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }


        public class Source1
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public char Char { get; set; }
            public DateTime DateTime { get; set; }
            public DateTime? DateTimeNullable1 { get; set; }
            public DateTime? DateTimeNullable2 { get; set; }
            public DateTimeOffset DateTimeOffset { get; set; }
            public decimal Decimal { get; set; }
            public float Float { get; set; }
            public int Int { get; set; }
            public int? Int1 { get; set; }
            public int Int2 { get; set; }
            public int? IntNullable1 { get; set; }
            public int? IntNullable2 { get; set; }
            public long Long { get; set; }
            public sbyte Sbyte { get; set; }
            public short Short { get; set; }
            public string String { get; set; }
            public ulong Ulong { get; set; }
            public ushort Ushort { get; set; }
        }


        public class Target1
        {
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public char Char { get; set; }
            public DateTime DateTime { get; set; }
            public DateTime? DateTimeNullable1 { get; set; }
            public DateTime? DateTimeNullable2 { get; set; }
            public DateTimeOffset DateTimeOffset { get; set; }
            public decimal Decimal { get; set; }
            public float Float { get; set; }
            public int Int { get; set; }
            public int? Int1 { get; set; }
            public int? Int2 { get; set; }
            public int IntNullable1 { get; set; }
            public int IntNullable2 { get; set; }
            public long Long { get; set; }
            public sbyte Sbyte { get; set; }
            public short Short { get; set; }
            public string String { get; set; }
            public ulong Ulong { get; set; }
            public ushort Ushort { get; set; }
        }
    }
}
