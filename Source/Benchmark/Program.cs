using System;
using Benchmark.Benchmarks;

namespace Benchmark
{
    internal class Program
    {
        private const int Iterations = 1000000;

        private static void Main()
        {
            var primitiveTypeBenchmark = new PrimitiveTypeBenchmark(Iterations);
            primitiveTypeBenchmark.Measure();

            Console.WriteLine("Press any key to Exit");
            Console.ReadLine();
        }
    }
}
