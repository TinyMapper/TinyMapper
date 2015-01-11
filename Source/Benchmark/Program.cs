using System;
using System.Diagnostics;
using TinyMapper.TypeConverters;

namespace Benchmark
{
    internal class Program
    {
        private const int Iterations = 1000000;

        private static void Main()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < Iterations; i++)
            {
                B.Enum value = PrimitiveTypeConverter.Convert<A.Enum, B.Enum>(A.Enum.Item1);
            }
            stopwatch.Stop();

            Console.WriteLine("Iterations: {0}, Time: {1}ms", Iterations, stopwatch.Elapsed.TotalMilliseconds);

            Console.WriteLine("Press any key to Exit");
            Console.ReadLine();
        }


        private static class A
        {
            public enum Enum
            {
                Item1,
                Item2,
                Item3
            }
        }


        private static class B
        {
            public enum Enum
            {
                Item1,
                Item2,
                Item3
            }
        }
    }
}
