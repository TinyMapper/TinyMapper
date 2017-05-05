using System;
using Benchmark.Benchmarks;

namespace Benchmark
{
    internal class Program
    {
        private const int Iterations = 100000;

        private static void Main()
        {
            var primitiveTypeBenchmark = new PrimitiveTypeBenchmark(Iterations);
            primitiveTypeBenchmark.Measure();

            var parallelPrimitiveTypeBenchmark = new ParallelPrimitiveTypeBenchmark(Iterations);
            parallelPrimitiveTypeBenchmark.Measure();

            var collectionBenchmark = new CollectionBenchmark(Iterations);
            collectionBenchmark.Measure();

            var parallelCollectionBenchmark = new ParallelCollectionBenchmark(Iterations);
            parallelCollectionBenchmark.Measure();

            Console.WriteLine("Press any key to Exit");
            Console.ReadLine();
        }
    }
}
