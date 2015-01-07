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
            DynamicAssembly.Save();
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
    }
}
