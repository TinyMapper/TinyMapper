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
            //            instance.Map();

            assemblyBuilder.Save("Test.dll");
        }

        private void EmitMethod(TypeBuilder typeBuilder)
        {
            MethodBuilder mapMethod = typeBuilder.DefineMethod("Map", MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), new[] { typeof(IEnumerable) });

            //                        var mapMethod = new DynamicMethod("Map", typeof(void), new[] { typeof(IEnumerable) }, typeof(ForeachSnippet).Module);

            ILGenerator il = mapMethod.GetILGenerator();
            LocalBuilder result = il.DeclareLocal(typeof(List<int>)); //0
            LocalBuilder item = il.DeclareLocal(typeof(object)); //1
            LocalBuilder enumeartor = il.DeclareLocal(typeof(IEnumerable)); //2
            LocalBuilder flag = il.DeclareLocal(typeof(bool)); //3
            LocalBuilder dispose = il.DeclareLocal(typeof(IDisposable)); //4

            Label labelStartLoop = il.DefineLabel();
            Label labelEndLoop = il.DefineLabel();
            Label labelMoveNext = il.DefineLabel();

            //Create result List
            ConstructorInfo constructorInfo = (typeof(List<int>).GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Newobj, constructorInfo);
            il.Emit(OpCodes.Stloc_0, result);

            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, typeof(IEnumerable).GetMethod("GetEnumerator"), Type.EmptyTypes);
            il.Emit(OpCodes.Stloc_2, enumeartor);

            il.BeginExceptionBlock();
            il.Emit(OpCodes.Br_S, labelMoveNext);

            il.MarkLabel(labelStartLoop);

            il.Emit(OpCodes.Ldloc_2);
            il.EmitCall(OpCodes.Callvirt, typeof(IEnumerator).GetProperty("Current").GetGetMethod(), Type.EmptyTypes);
            il.Emit(OpCodes.Stloc_1, item);


            il.MarkLabel(labelMoveNext);
            il.Emit(OpCodes.Ldloc_2);
            il.EmitCall(OpCodes.Callvirt, typeof(IEnumerator).GetMethod("MoveNext"), Type.EmptyTypes);
            il.Emit(OpCodes.Stloc_3, item);
            il.Emit(OpCodes.Ldloc_3);
            il.Emit(OpCodes.Brtrue_S, labelStartLoop);
            il.Emit(OpCodes.Leave_S, labelEndLoop);

            il.BeginFinallyBlock();


            il.EndExceptionBlock();
            il.MarkLabel(labelEndLoop);

            il.Emit(OpCodes.Ret);

            //            var methodDelegate = (Action<IEnumerable>)mapMethod.CreateDelegate(typeof(Action<IEnumerable>));
            //            methodDelegate(new List<int>());
        }
    }


    public abstract class ForeachBase
    {
        public abstract void Map(IEnumerable value);
    }


    //        protected List<TTargetItem> EnumerableToListTemplate<TTargetItem>(IEnumerable source)
    //        {
    //            var result = new List<TTargetItem>();
    //            foreach (object item in source)
    //            {
    //                result.Add((TTargetItem)ConvertItem(item));
    //            }
    //            return result;
    //        }
}
