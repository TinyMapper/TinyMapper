using System;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Mappers;
using Nelibur.ObjectMapper.Mappers.Caches;
using Nelibur.ObjectMapper.Mappers.Classes;
using Xunit;

namespace UnitTests.Mappers.Classes
{
    public sealed class ClassMapperTests
    {
        [Fact]
        public void Map_PrimitiveField_Success()
        {
            var builder = new ClassMapperBuilder(new MapperCache(), new MappingBuilderConfigStub());
            Mapper mapper = builder.Build(new TypePair(typeof(SourceWithFields), typeof(TargetWithFields)));

            var source = new SourceWithFields
            {
                Bool = true,
                Byte = 0,
                Char = 'a',
                Decimal = 4.0m,
                Float = 2.0f,
                Int = 9,
                Long = 2,
                Sbyte = 8,
                Short = 1,
                String = "test",
                Ulong = 3,
                Ushort = 7
            };

            var actual = (TargetWithFields)mapper.Map(source);

            Assert.Equal(source.Bool, actual.Bool);
            Assert.Equal(source.Byte, actual.Byte);
            Assert.Equal(source.Char, actual.Char);
            Assert.Equal(source.Decimal, actual.Decimal);
            Assert.Equal(source.Float, actual.Float);
            Assert.Equal(source.Int, actual.Int);
            Assert.Equal(source.Long, actual.Long);
            Assert.Equal(source.Sbyte, actual.Sbyte);
            Assert.Equal(source.Short, actual.Short);
            Assert.Equal(source.String, actual.String);
            Assert.Equal(source.Ulong, actual.Ulong);
            Assert.Equal(source.Ushort, actual.Ushort);
        }

        [Fact]
        public void Map_PrimitiveProperty_Success()
        {
            var builder = new ClassMapperBuilder(new MapperCache(), new MappingBuilderConfigStub());
            Mapper mapper = builder.Build(new TypePair(typeof(SourceWithProperties), typeof(TargetWithProperties)));

            var source = new SourceWithProperties
            {
                Bool = true,
                Byte = 0,
                Char = 'a',
                Decimal = 4.0m,
                Float = 2.0f,
                Int = 9,
                Long = 2,
                Sbyte = 8,
                Short = 1,
                String = "test",
                Ulong = 3,
                Ushort = 7,
                DateTime = new DateTime(1990, 1, 1),
                DateTimeOffset = new DateTimeOffset(new DateTime(1998, 3, 5)),
                DateTimeNullable = null,
                DateTimeNullable1 = new DateTime(2020, 2, 4)
            };

            var actual = (TargetWithProperties)mapper.Map(source);

            Assert.Equal(source.Bool, actual.Bool);
            Assert.Equal(source.Byte, actual.Byte);
            Assert.Equal(source.Char, actual.Char);
            Assert.Equal(source.Decimal, actual.Decimal);
            Assert.Equal(source.Float, actual.Float);
            Assert.Equal(source.Int, actual.Int);
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

        public class SourceWithFields
        {
            public bool Bool;
            public byte Byte;
            public char Char;
            public decimal Decimal;
            public float Float;
            public int Int;
            public long Long;
            public sbyte Sbyte;
            public short Short;
            public string String;
            public ulong Ulong;
            public ushort Ushort;
        }

        public class TargetWithFields
        {
            public bool Bool;
            public byte Byte;
            public char Char;
            public decimal Decimal;
            public float Float;
            public int Int;
            public long Long;
            public sbyte Sbyte;
            public short Short;
            public string String;
            public ulong Ulong;
            public ushort Ushort;
        }

        public class SourceWithProperties
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
            public long Long { get; set; }
            public sbyte Sbyte { get; set; }
            public short Short { get; set; }
            public string String { get; set; }
            public ulong Ulong { get; set; }
            public ushort Ushort { get; set; }
        }

        public class TargetWithProperties
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
            public long Long { get; set; }
            public sbyte Sbyte { get; set; }
            public short Short { get; set; }
            public string String { get; set; }
            public ulong Ulong { get; set; }
            public ushort Ushort { get; set; }
        }
    }
}
