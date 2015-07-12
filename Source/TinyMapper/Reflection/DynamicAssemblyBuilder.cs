using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Nelibur.ObjectMapper.Reflection
{
    internal class DynamicAssemblyBuilder
    {
        internal const string AssemblyName = "DynamicTinyMapper";
//        private const string AssemblyNameFileName = AssemblyName + ".dll";
        private static readonly DynamicAssembly _dynamicAssembly = new DynamicAssembly();
//        private static AssemblyBuilder _assemblyBuilder;

        public static IDynamicAssembly Get()
        {
            return _dynamicAssembly;
        }

        private sealed class DynamicAssembly : IDynamicAssembly
        {
            private readonly ModuleBuilder _moduleBuilder;

            public DynamicAssembly()
            {
                var assemblyName = new AssemblyName(AssemblyName);
                AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
//                _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

                _moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
//                _moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name, AssemblyNameFileName, true);
            }

            public TypeBuilder DefineType(string typeName, Type parentType)
            {
                return _moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Sealed, parentType, null);
            }

            public void Save()
            {
//                _assemblyBuilder.Save(AssemblyNameFileName);
            }
        }
    }
}
