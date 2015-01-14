using System;
using System.Diagnostics;
using TinyMapper;

namespace Benchmark
{
    internal class Program
    {
        private const int Iterations = 100000;

        private static void Main()
        {
            ObjectMapper.Bind<Class1, Class2>();

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < Iterations; i++)
            {
                var source = new Class1
                {
                    Field = 10,
                };
                var cl2 = new Class2 { Field = 3 };
                Class2 t = ObjectMapper.Project(source, cl2);
            }
            stopwatch.Stop();

            Console.WriteLine("Iterations: {0}, Time: {1}ms", Iterations, stopwatch.Elapsed.TotalMilliseconds);

            Console.WriteLine("Press any key to Exit");
            Console.ReadLine();
        }
    }


    public class Class1
    {
        public int Field;
    }


    public class Class2
    {
        public int Field;
    }
}
