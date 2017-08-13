using System;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using Nelibur.ObjectMapper;

namespace Benchmark
{
    public class PrimitiveTypeBenchmark
    {
        private readonly SourceWithPrimitiveTypes _source = CreateSource();

        public PrimitiveTypeBenchmark()
        {
            InitTinyMapper();
            InitTinyAutoMapper();
        }

        private void InitTinyMapper()
        {
            TinyMapper.Bind<SourceWithPrimitiveTypes, TargetWithPrimitiveTypes>();
        }

        private void InitTinyAutoMapper()
        {
            Mapper.Initialize(x => x.CreateMap<SourceWithPrimitiveTypes, TargetWithPrimitiveTypes>());
        }

        private static SourceWithPrimitiveTypes CreateSource()
        {
            return new SourceWithPrimitiveTypes
            {
                FirstName = "John",
                LastName = "Doe",
                Nickname = "TinyMapper",
                Email = "support@TinyMapper.net",
                Short = 3,
                Long = 10,
                Int = 5,
                Float = 4.9f,
                Decimal = 4.0m,
                DateTime = DateTime.Now,
                Char = 'a',
                Bool = true,
                Byte = 0
            };
        }

        [Benchmark]
        public void BenchmarkTinyMapper()
        {
            TinyMapper.Map<SourceWithPrimitiveTypes, TargetWithPrimitiveTypes>(_source);
        }

        [Benchmark]
        public void BenchmarkAutoMapper()
        {
            Mapper.Map<TargetWithPrimitiveTypes>(_source);
        }
    }


    public sealed class SourceWithPrimitiveTypes
    {
        public bool Bool { get; set; }
        public byte Byte { get; set; }
        public char Char { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Decimal { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public float Float { get; set; }
        public int Int { get; set; }
        public string LastName { get; set; }
        public long Long { get; set; }
        public string Nickname { get; set; }
        public short Short { get; set; }
    }


    public sealed class TargetWithPrimitiveTypes
    {
        public bool Bool { get; set; }
        public byte Byte { get; set; }
        public char Char { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Decimal { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public float Float { get; set; }
        public int Int { get; set; }
        public string LastName { get; set; }
        public long Long { get; set; }
        public string Nickname { get; set; }
        public short Short { get; set; }
    }
}
