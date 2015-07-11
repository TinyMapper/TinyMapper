using System;

namespace Benchmark.Benchmarks
{
    public abstract class Benchmark
    {
        protected readonly int _iterations;

        protected Benchmark(int iterations)
        {
            _iterations = iterations;
            InitMappers();
        }

        public void Measure()
        {
            string inputInfo = MeasureMapperInputInfo();
            if (string.IsNullOrWhiteSpace(inputInfo))
            {
                Console.WriteLine("Iterations: {0}", _iterations);
            }
            else
            {
                Console.WriteLine("Iterations: {0}, {1}", _iterations, inputInfo);
            }

            TimeSpan handmade = MeasureHandmade();
            Console.WriteLine("Handmade: {0}ms", handmade.TotalMilliseconds);

            TimeSpan tinyMapper = MeasureTinyMapper();
            Console.WriteLine("TinyMapper: {0}ms", tinyMapper.TotalMilliseconds);

            TimeSpan autoMapper = MeasureAutoMapper();
            Console.WriteLine("AutoMapper: {0}ms", autoMapper.TotalMilliseconds);

            Console.WriteLine(Environment.NewLine);
        }

        protected abstract void InitMappers();

        protected abstract TimeSpan MeasureAutoMapper();
        protected abstract TimeSpan MeasureHandmade();

        protected virtual string MeasureMapperInputInfo()
        {
            return string.Empty;
        }

        protected abstract TimeSpan MeasureTinyMapper();
    }
}
