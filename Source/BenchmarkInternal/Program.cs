using System;
using BenchmarkDotNet.Running;

namespace BenchmarkInternal
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmark>();

            Console.ReadKey();
        }
    }
}