using System;
using System.Diagnostics;
using AutoMapper;
using Nelibur.ObjectMapper;
using System.Linq;

namespace Benchmark.Benchmarks
{
    public sealed class ParallelPrimitiveTypeBenchmark : Benchmark
    {
        public ParallelPrimitiveTypeBenchmark(int iterations) : base(iterations)
        {
        }

        protected override string MeasureMapperInputInfo()
        {
            return "as Parallel";
        }

        protected override void InitMappers()
        {
            TinyMapper.Bind<Source, Target>();
            Mapper.CreateMap<Source, Target>();
        }

        protected override TimeSpan MeasureAutoMapper()
        {
            Source source = CreateSource();
            Mapper.Map<Target>(source);

            Stopwatch stopwatch = Stopwatch.StartNew();

            Enumerable.Range(0, _iterations).AsParallel().ForAll(n => 
            {
                var target = Mapper.Map<Target>(source);
            });

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        protected override TimeSpan MeasureHandmade()
        {
            Source source = CreateSource();

            Stopwatch stopwatch = Stopwatch.StartNew();

            Enumerable.Range(0, _iterations).AsParallel().ForAll(n =>
            {
                var target = new Target();
                target = HandmadeMap(source, target);
            });

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        protected override TimeSpan MeasureTinyMapper()
        {
            Source source = CreateSource();
            TinyMapper.Map<Target>(source);

            Stopwatch stopwatch = Stopwatch.StartNew();

            Enumerable.Range(0, _iterations).AsParallel().ForAll(n =>
            {
                var target = TinyMapper.Map<Target>(source);
            });

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private static Source CreateSource()
        {
            return new Source
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

        private static Target HandmadeMap(Source source, Target target)
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
