using System;
using System.Collections.Generic;
using DynamicTinyMapper.ClassMappers;
using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Reflection;

namespace DebugDynamicAssembly
{
    public class Program
    {
        private static void Bind()
        {
            TinyMapper.Bind<Class1, Class2>();
            DynamicAssemblyBuilder.Get().Save();
        }

        private static void Main(string[] args)
        {
            try
            {
//                Bind();
                Map();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }

        private static void Map()
        {
            var source = new Class1
            {
                Bools = new List<bool> { true, false }
            };
            object result = new Mapper5496b8a5fe7e48009e9492e2665ec890().Map(source);
        }
    }


    public class Class1
    {
        //            public int Field;
        //            public int Property { get; set; }
        //            public List<int> List { get; set; }
        //            public Class3 Class3 { get; set; }
        //            public int[] Array { get; set; }
        public List<bool> Bools { get; set; }
    }


    public class Class2
    {
        //            public int Field;
        //            public int Property { get; set; }
        //            public List<int> List { get; set; }
        //            public Class3 Class3 { get; set; }
        //            public int[] Array { get; set; }
        public List<bool> Bools { get; set; }
    }
}
