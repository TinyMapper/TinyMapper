using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using TinyMapper;

namespace Benchmark
{
    internal class Program
    {
        private const int Iterations = 100;

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
                List = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 }
                //                Field1 = 1,
                //                Field2 = 2,
                //                Field3 = 3,
                //                Field4 = 4,
                //                Field5 = 5,
                //                Property1 = "1",
                //                Property2 = "2",
                //                Property3 = "3",
                //                Property4 = "4",
                //                Property5 = "5"
            };
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
            const int Repeat = 3;
            for (int i = 0; i < Repeat; i++)
            {
                TinyMapper();
            }
            Console.WriteLine();
            for (int i = 0; i < Repeat; i++)
            {
                AutoMapper();
            }

            Console.WriteLine("Press any key to Exit");
            Console.ReadLine();
        }

        private static void TinyMapper()
        {
            Class1 source = CreateSource();
            var temp = ObjectMapper.Map<Class2>(source);

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < Iterations; i++)
            {
                var t = ObjectMapper.Map<Class2>(source);
            }
            stopwatch.Stop();
            Console.WriteLine("TinyMapper: Iterations: {0}, Time: {1}ms", Iterations, stopwatch.Elapsed.TotalMilliseconds);
        }
    }


    public class Class1
    {
        //        public int Field1;
        public List<int> List { get; set; }
        //        public int Field2;
        //        public int Field3;
        //        public int Field4;
        //        public int Field5;
        //
        //        public string Property1 { get; set; }
        //        public string Property2 { get; set; }
        //        public string Property3 { get; set; }
        //        public string Property4 { get; set; }
        //        public string Property5 { get; set; }
    }


    public class Class2
    {
        //        public int Field1;
        public List<int> List { get; set; }
        //        public int Field2;
        //        public int Field3;
        //        public int Field4;
        //        public int Field5;
        //
        //        public string Property1 { get; set; }
        //        public string Property2 { get; set; }
        //        public string Property3 { get; set; }
        //        public string Property4 { get; set; }
        //        public string Property5 { get; set; }
    }
}
