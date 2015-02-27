using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
{
    public class ForTests
    {
        [Fact]
        public void Test()
        {
            TinyMapper.Bind<Class1, Class2>();
            var source = new Class1
            {
                //                Field = 10,
                //                Property = 4
                //                List = new List<int> { 1, 2 }
                //                Class3 = new Class3 { Id = 1 }
                //                Array = new int[] { 0, 1 }
                Bools = new List<bool> { true, false }
            };
            //            DynamicAssemblyBuilder.Get().Save();
            var target = TinyMapper.Map<Class2>(source);
            //            CallDynamicMethod();
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
}
