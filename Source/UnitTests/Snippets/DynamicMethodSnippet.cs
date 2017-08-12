#if !COREFX
using System;
using System.Reflection.Emit;
using Xunit;

namespace UnitTests.Snippets
{
    public sealed class DynamicMethodSnippet
    {
        [Fact]
        public void Snippet()
        {
            CallDynamicMethod();
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


        public sealed class A
        {
        }


        public sealed class B
        {
            public void Method()
            {
                var x = Cast<A>(this);
                //                var x = this as A;
            }

            private static T Cast<T>(object value)
                where T : class
            {
                var method = new DynamicMethod("CastMethod", typeof(T), new[] { typeof(object) });
                ILGenerator il = method.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Isinst, typeof(T));
                il.Emit(OpCodes.Ret);

                var methodDelegate = (Func<object, T>)method.CreateDelegate(typeof(Func<object, T>));
                return methodDelegate(value);
            }
        }
    }
}
#endif
