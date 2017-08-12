#if !COREFX
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Xunit;

namespace UnitTests.Snippets
{
    public sealed class ForeachSnippet
    {
        [Fact]
        public void Snippet()
        {
            var assemblyName = new AssemblyName("Test");
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, "Test.dll", true);

            TypeBuilder typeBuilder = moduleBuilder.DefineType("ForeachType", TypeAttributes.Sealed | TypeAttributes.Public, typeof(ForeachBase));

            EmitMethod(typeBuilder);

            Type type = typeBuilder.CreateType();
            var instance = (ForeachBase)Activator.CreateInstance(type);

            instance.Map(new List<int>());

            assemblyBuilder.Save("Test.dll");
        }

        private void EmitMethod(TypeBuilder typeBuilder)
        {
            MethodBuilder mapMethod = typeBuilder.DefineMethod("Map", MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), new[] { typeof(IEnumerable) });

            ILGenerator il = mapMethod.GetILGenerator();
            LocalBuilder result = il.DeclareLocal(typeof(List<int>)); //0
            LocalBuilder item = il.DeclareLocal(typeof(object)); //1
            LocalBuilder enumeartor = il.DeclareLocal(typeof(IEnumerator)); //2
            LocalBuilder dispose = il.DeclareLocal(typeof(IDisposable)); //3

            Label labelWhile = il.DefineLabel();
            Label labelReturn = il.DefineLabel();
            Label labelMoveNext = il.DefineLabel();
            Label labelEndFinally = il.DefineLabel();

            //Create result List
            ConstructorInfo constructorInfo = (typeof(List<int>).GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Newobj, constructorInfo);
            il.Emit(OpCodes.Stloc_0, result);

            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, typeof(IEnumerable).GetMethod("GetEnumerator"), Type.EmptyTypes);
            il.Emit(OpCodes.Stloc_2, enumeartor);

            il.BeginExceptionBlock();
            il.Emit(OpCodes.Br_S, labelMoveNext);
            il.MarkLabel(labelWhile);

            il.Emit(OpCodes.Ldloc_2);
            il.EmitCall(OpCodes.Callvirt, typeof(IEnumerator).GetProperty("Current").GetGetMethod(), Type.EmptyTypes);
            il.Emit(OpCodes.Stloc_1, item);

            il.MarkLabel(labelMoveNext);
            il.Emit(OpCodes.Ldloc_2);
            il.EmitCall(OpCodes.Callvirt, typeof(IEnumerator).GetMethod("MoveNext"), Type.EmptyTypes);
            il.Emit(OpCodes.Brtrue_S, labelWhile);

            il.BeginFinallyBlock();

            il.Emit(OpCodes.Ldloc_2);
            il.Emit(OpCodes.Isinst, typeof(IDisposable));
            il.Emit(OpCodes.Stloc_3, dispose);
            il.Emit(OpCodes.Ldloc_3);
            il.Emit(OpCodes.Brfalse_S, labelEndFinally);

            il.Emit(OpCodes.Ldloc_3);
            il.EmitCall(OpCodes.Callvirt, typeof(IDisposable).GetMethod("Dispose"), Type.EmptyTypes);

            il.MarkLabel(labelEndFinally);
            il.EndExceptionBlock();

            il.MarkLabel(labelReturn);
            il.Emit(OpCodes.Ret);
        }
    }


    public abstract class ForeachBase
    {
        public abstract void Map(IEnumerable value);
    }


    //Result Code
    //public sealed class ForeachType : ForeachBase
    //{
    //    public override void Map(IEnumerable enumerable1)
    //    {
    //        List<int> list = new List<int>();
    //        foreach (object obj2 in enumerable1)
    //        {
    //        }
    //    }
    //}
}
#endif
