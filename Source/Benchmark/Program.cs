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
                FirstName = "John",
                LastName = "Doe",
                Nickname = "TinyMapper",
                Email = "support@TinyMapper.net",
                Short = 3,
                Long = 10,
                Int = 5,
                Float = 4.9f,
                Decimal = 4.0m,
                DateTime = DateTime.Now,
                Char = 'a',
                Bool = true,
                Byte = 0
            };
        }

        private static Class2 HandmadeMap(Class1 source, Class2 target)
        {
            //            target.List = new List<int>(source.List);
            target.Bool = source.Bool;
            target.Byte = source.Byte;
            target.Char = source.Char;
            target.DateTime = source.DateTime;
            target.Decimal = source.Decimal;
            target.Float = source.Float;
            target.Int = source.Int;
            target.Long = source.Long;
            target.Short = source.Short;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.Nickname = source.Nickname;
            target.Email = source.Email;
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
        //        public List<int> List { get; set; }
        public bool Bool { get; set; }
        public byte Byte { get; set; }
        public char Char { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Decimal { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public float Float { get; set; }
        public int Int { get; set; }
        public string LastName { get; set; }
        public long Long { get; set; }
        public string Nickname { get; set; }
        public short Short { get; set; }
    }


    public class Class2
    {
        //        public List<bool> Bools { get; set; }
        //        public List<int> List { get; set; }
        public bool Bool { get; set; }
        public byte Byte { get; set; }
        public char Char { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Decimal { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public float Float { get; set; }
        public int Int { get; set; }
        public string LastName { get; set; }
        public long Long { get; set; }
        public string Nickname { get; set; }
        public short Short { get; set; }
    }
}
