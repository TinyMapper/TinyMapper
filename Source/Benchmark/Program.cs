using System;
using System.Diagnostics;
using AutoMapper;
using TinyMapper;

namespace Benchmark
{
    internal class Program
    {
        private const int Iterations = 1000000;

        private static void AutoMapper()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            var source = new Class1 { Field = 10 };

            for (int i = 0; i < Iterations; i++)
            {
                var t = Mapper.Map<Class2>(source);
            }
            stopwatch.Stop();
            Console.WriteLine("AutoMapper: Iterations: {0}, Time: {1}ms", Iterations, stopwatch.Elapsed.TotalMilliseconds);
        }

        private static void HandmadeMapper()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            var source = new Class1 { Field = 10 };

            for (int i = 0; i < Iterations; i++)
            {
                var t = new Class2
                {
                    Field = source.Field
                };
            }
            stopwatch.Stop();
            Console.WriteLine("Handmade: Iterations: {0}, Time: {1}ms", Iterations, stopwatch.Elapsed.TotalMilliseconds);
        }

        private static void Initialise()
        {
            ObjectMapper.Bind<Class1, Class2>();
            Mapper.CreateMap<Class1, Class2>();
        }

        private static void Main()
        {
            Initialise();

            //            HandmadeMapper();
            TinyMapper();
            AutoMapper();

            Console.WriteLine("Press any key to Exit");
            Console.ReadLine();
        }

        private static void TinyMapper()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            var source = new Class1 { Field = 10 };

            for (int i = 0; i < Iterations; i++)
            {
                var t = ObjectMapper.Project<Class2>(source);
            }
            stopwatch.Stop();
            Console.WriteLine("TinyMapper: Iterations: {0}, Time: {1}ms", Iterations, stopwatch.Elapsed.TotalMilliseconds);
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
