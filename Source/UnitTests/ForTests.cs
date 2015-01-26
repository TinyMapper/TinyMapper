using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TinyMappers;
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
                //                                Field = 10,
                //                Property = 4
                List = new List<int> { 1 }
            };
            var t = TinyMapper.Map<Class2>(source);
            //            CallDynamicMethod();
        }

        private static void CallDynamicMethod()
        {
            var multiplyMethod = new DynamicMethod("MultiplyMethod", typeof(int), new[] { typeof(int) },
                typeof(ForTests).Module);

            ILGenerator multiplyMethodIL = multiplyMethod.GetILGenerator();

            multiplyMethodIL.Emit(OpCodes.Ldarg_0);
            multiplyMethodIL.Emit(OpCodes.Ldc_I4, 2);
            multiplyMethodIL.Emit(OpCodes.Mul);
            multiplyMethodIL.Emit(OpCodes.Ret);

            var calculateMethod = new DynamicMethod("CalculateMethod", typeof(int), new[] { typeof(int), typeof(int) },
                typeof(ForTests).Module);

            ILGenerator calculateMethodIL = calculateMethod.GetILGenerator();

            calculateMethodIL.Emit(OpCodes.Ldarg_0);
            calculateMethodIL.Emit(OpCodes.Ldarg_1);
            calculateMethodIL.Emit(OpCodes.Mul);
            calculateMethodIL.Emit(OpCodes.Call, multiplyMethod);
            calculateMethodIL.Emit(OpCodes.Ret);

            var calcMethodDelegate = (Func<int, int, int>)calculateMethod.CreateDelegate(typeof(Func<int, int, int>));
            int result = calcMethodDelegate(10, 10);

            Console.WriteLine(result);
        }


        public class Class1
        {
            //                        public int Field;
            //            public int Property { get; set; }
            public List<int> List { get; set; }
        }


        public class Class2
        {
            //                        public int Field;
            //            public int Property { get; set; }
            public List<int> List { get; set; }
        }
    }
}
