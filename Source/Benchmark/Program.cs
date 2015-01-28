using System;
using System.Diagnostics;
using AutoMapper;
using TinyMappers;

namespace Benchmark
{
    internal class Program
    {
        private const int Iterations = 1000000;

        private static void AutoMapper()
        {
            Class1 source = CreateSource();
            var temp = Mapper.Map<Class2>(source);

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < Iterations; i++)
            {
                var t = Mapper.Map<Class2>(source);
            }
            stopwatch.Stop();
            Console.WriteLine("AutoMapper: Iterations: {0}, Time: {1}ms", Iterations, stopwatch.Elapsed.TotalMilliseconds);
        }

        private static Class1 CreateSource()
        {
            return new Class1
            {
                //                List = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 },
                Int1 = 1,
                Int2 = 2,
                Int3 = 3,
                Int4 = 4,
                Int5 = 5,
                String1 = "1",
                String2 = "2",
                String3 = "3",
                String4 = "4",
                String5 = "5"
            };
        }

        private static void Initialise()
        {
            TinyMapper.Bind<Class1, Class2>();
            Mapper.CreateMap<Class1, Class2>();
        }

        private static void Main()
        {
            Initialise();

            //            HandmadeMapper();
            const int Repeat = 3;
            for (int i = 0; i < Repeat; i++)
            {
                TinyMapperTest();
            }
            Console.WriteLine();
            for (int i = 0; i < Repeat; i++)
            {
                AutoMapper();
            }

            Console.WriteLine("Press any key to Exit");
            Console.ReadLine();
        }

        private static void TinyMapperTest()
        {
            Class1 source = CreateSource();
            var temp = TinyMapper.Map<Class2>(source);

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < Iterations; i++)
            {
                var t = TinyMapper.Map<Class2>(source);
            }
            stopwatch.Stop();
            Console.WriteLine("TinyMapper: Iterations: {0}, Time: {1}ms", Iterations, stopwatch.Elapsed.TotalMilliseconds);
        }
    }


    public class Class1
    {
        public int Int1;
        public int Int2;
        public int Int3;
        public int Int4;
        public int Int5;
        //        public List<int> List { get; set; }

        public string String1 { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; }
        public string String4 { get; set; }
        public string String5 { get; set; }
    }


    public class Class2
    {
        public int Int1;
        public int Int2;
        public int Int3;
        public int Int4;
        public int Int5;
        //        public List<int> List { get; set; }

        public string String1 { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; }
        public string String4 { get; set; }
        public string String5 { get; set; }
    }
}
