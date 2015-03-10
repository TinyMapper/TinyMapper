using System;

namespace Benchmark.Benchmarks
{
    public abstract class Benchmark
    {
        protected readonly int _iterations;

        protected Benchmark(int iterations)
        {
            _iterations = iterations;
        }

        public void Measure()
        {
            Console.WriteLine("Iterations: {0}", _iterations);

            TimeSpan handmade = MeasureHandmade();
            Console.WriteLine("Handmade: {0}ms", handmade.TotalMilliseconds);

            TimeSpan tinyMapper = MeasureTinyMapper();
            Console.WriteLine("TinyMapper: {0}ms", tinyMapper.TotalMilliseconds);

            TimeSpan autoMapper = MeasureAutoMapper();
            Console.WriteLine("AutoMapper: {0}ms", autoMapper.TotalMilliseconds);

            Console.WriteLine(Environment.NewLine);
        }

        protected abstract TimeSpan MeasureAutoMapper();
        protected abstract TimeSpan MeasureHandmade();
        protected abstract TimeSpan MeasureTinyMapper();
    }
}
