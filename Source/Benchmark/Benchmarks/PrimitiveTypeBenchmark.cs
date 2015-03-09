using System;
using System.Diagnostics;
using AutoMapper;
using Nelibur.ObjectMapper;

namespace Benchmark.Benchmarks
{
    public sealed class PrimitiveTypeBenchmark : Benchmark
    {
        public PrimitiveTypeBenchmark(int iterations) : base(iterations)
        {
            InitMappers();
        }

        protected override string Name
        {
            get { return "PrimitiveType"; }
        }

        protected override TimeSpan MeasureAutoMapper()
        {
            Class1 source = CreateSource();
            Mapper.Map<Class2>(source);

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < _iterations; i++)
            {
                var target = Mapper.Map<Class2>(source);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        protected override TimeSpan MeasureHandmade()
        {
            Class1 source = CreateSource();

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < _iterations; i++)
            {
                var target = new Class2();
                target = HandmadeMap(source, target);
            }

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        protected override TimeSpan MeasureTinyMapper()
        {
            Class1 source = CreateSource();
            TinyMapper.Map<Class2>(source);

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < _iterations; i++)
            {
                var target = TinyMapper.Map<Class2>(source);
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private static Class1 CreateSource()
        {
            return new Class1
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

        private static Class2 HandmadeMap(Class1 source, Class2 target)
        {
            target.Bool = source.Bool;
            target.Byte = source.Byte;
            target.Char = source.Char;
            target.DateTime = source.DateTime;
            target.Decimal = source.Decimal;
            target.Float = source.Float;
            target.Int = source.Int;
            target.Long = source.Long;
            target.Short = source.Short;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.Nickname = source.Nickname;
            target.Email = source.Email;
            return target;
        }

        private static void InitMappers()
        {
            TinyMapper.Bind<Class1, Class2>();
            Mapper.CreateMap<Class1, Class2>();
        }


        public class Class1
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


        public class Class2
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
