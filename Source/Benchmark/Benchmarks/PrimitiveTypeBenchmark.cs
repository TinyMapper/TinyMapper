using System;

namespace Benchmark.Benchmarks
{
    public sealed class PrimitiveTypeBenchmark
    {
        public class Source
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


        public class Target
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
}
