using System;
using BenchmarkDotNet.Running;

namespace Benchmark
{
    public class Program
    {
        public static void Main()
        {
//            BenchmarkRunner.Run<PrimitiveTypeBenchmark>();
            BenchmarkRunner.Run<CollectionBenchmark>();

            Console.ReadKey();
        }
    }
}
