using System;
using System.Diagnostics;
using AutoMapper;
using Nelibur.ObjectMapper;

namespace Benchmark
{
    internal class Program
    {
        private const int Iterations = 1000000;

        private static void AutoMapperTest(int repeat = 1)
        {
            for (int measure = 0; measure < repeat; measure++)
            {
                Class1 source = CreateSource();
                Mapper.Map<Class2>(source);

                Stopwatch stopwatch = Stopwatch.StartNew();

                for (int i = 0; i < Iterations; i++)
                {
                    var target = Mapper.Map<Class2>(source);
                }
                stopwatch.Stop();
                Console.WriteLine("AutoMapper: Iterations: {0}, Time: {1}ms", Iterations, stopwatch.Elapsed.TotalMilliseconds);
            }
            Console.WriteLine();
        }

        private static Class1 CreateSource()
        {
            return new Class1
            {
                //                Bools = new List<bool> { true, false }
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

        private static Class2 HandmadeMap(Class1 source, Class2 target)
        {
            target.Int1 = source.Int1;
            target.Int2 = source.Int2;
            target.Int3 = source.Int3;
            target.Int4 = source.Int4;
            target.Int5 = source.Int5;
            target.String1 = source.String1;
            target.String2 = source.String2;
            target.String3 = source.String3;
            target.String4 = source.String4;
            target.String5 = source.String5;
            //            target.List = new List<int>(source.List);
            return target;
        }

        private static void HandmadeTest(int repeat = 1)
        {
            for (int measure = 0; measure < repeat; measure++)
            {
                Class1 source = CreateSource();

                Stopwatch stopwatch = Stopwatch.StartNew();

                for (int i = 0; i < Iterations; i++)
                {
                    var target = new Class2();
                    target = HandmadeMap(source, target);
                }

                stopwatch.Stop();
                Console.WriteLine("Handmade: Iterations: {0}, Time: {1}ms", Iterations, stopwatch.Elapsed.TotalMilliseconds);
            }
            Console.WriteLine();
        }

        private static void InitMappers()
        {
            TinyMapper.Bind<Class1, Class2>();
            Mapper.CreateMap<Class1, Class2>();
        }

        private static void Main()
        {
            const int Repeat = 2;

            InitMappers();

            HandmadeTest(Repeat);

            TinyMapperTest(Repeat);

            AutoMapperTest(Repeat);

            Console.WriteLine("Press any key to Exit");
            Console.ReadLine();
        }

        private static void TinyMapperTest(int repeat = 1)
        {
            for (int measure = 0; measure < repeat; measure++)
            {
                Class1 source = CreateSource();
                TinyMapper.Map<Class2>(source);

                Stopwatch stopwatch = Stopwatch.StartNew();

                for (int i = 0; i < Iterations; i++)
                {
                    var target = TinyMapper.Map<Class2>(source);
                }
                stopwatch.Stop();
                Console.WriteLine("TinyMapper: Iterations: {0}, Time: {1}ms", Iterations, stopwatch.Elapsed.TotalMilliseconds);
            }
            Console.WriteLine();
        }
    }


    public class Class1
    {
        //        public List<bool> Bools { get; set; }
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
        public int Int6;
        //        public List<bool> Bools { get; set; }
        //        public List<int> List { get; set; }
        public string String1 { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; }
        public string String4 { get; set; }
        public string String5 { get; set; }
    }
}
