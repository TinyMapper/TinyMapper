using System;
using System.Reflection.Emit;
using TinyMapper;
using Xunit;

namespace UnitTests
{
    public class ForTests
    {
        [Fact]
        public void Test()
        {
            ObjectMapper.Bind<Class1, Class2>();
            var source = new Class1
            {
                Field = 10,
            };
            var cl2 = new Class2 { Field = 3 };
            Class2 t = ObjectMapper.Project(source, cl2);

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
            public int Field;
        }


        public class Class2
        {
            public int Field;
        }
    }
}
