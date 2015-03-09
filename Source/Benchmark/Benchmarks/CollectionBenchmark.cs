using System;

namespace Benchmark.Benchmarks
{
    public sealed class CollectionBenchmark : Benchmark
    {
        public CollectionBenchmark(int iterations) : base(iterations)
        {
        }

        protected override TimeSpan MeasureAutoMapper()
        {
            throw new NotImplementedException();
        }

        protected override TimeSpan MeasureHandmade()
        {
            throw new NotImplementedException();
        }

        protected override TimeSpan MeasureTinyMapper()
        {
            throw new NotImplementedException();
        }
    }
}
