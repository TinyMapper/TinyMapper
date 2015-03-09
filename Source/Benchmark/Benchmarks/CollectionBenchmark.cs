using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using Nelibur.ObjectMapper;

namespace Benchmark.Benchmarks
{
    public sealed class CollectionBenchmark : Benchmark
    {
        public CollectionBenchmark(int iterations) : base(iterations)
        {
            InitMappers();
        }

        protected override string Name
        {
            get { return "Collection"; }
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
                Ints = new List<int> { 1, 2, 3, 4, 5 }
            };
        }

        private static void InitMappers()
        {
            TinyMapper.Bind<Class1, Class2>();
            Mapper.CreateMap<Class1, Class2>();
        }

        private Class2 HandmadeMap(Class1 source, Class2 target)
        {
            target.Ints = new List<int>();
            foreach (int item in source.Ints)
            {
                target.Ints.Add(item);
            }
            return target;
        }


        public class Class1
        {
            public List<int> Ints { get; set; }
        }


        public class Class2
        {
            public List<int> Ints { get; set; }
        }
    }
}
