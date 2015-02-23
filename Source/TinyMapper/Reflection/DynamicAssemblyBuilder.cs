using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Nelibur.ObjectMapper.Reflection
{
    internal class DynamicAssemblyBuilder
    {
        internal const string AssemblyName = "DynamicTinyMapper";
        private const string AssemblyNameFileName = AssemblyName + ".dll";
        private static readonly DynamicAssembly _dynamicAssembly = new DynamicAssembly();

        public static IDynamicAssembly Get()
        {
            return _dynamicAssembly;
        }


        private sealed class DynamicAssembly : IDynamicAssembly
        {
            private readonly AssemblyBuilder _assemblyBuilder;
            private readonly ModuleBuilder _moduleBuilder;

            public DynamicAssembly()
            {
                var assemblyName = new AssemblyName(AssemblyName);
                //                _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName,  AssemblyBuilderAccess.RunAndSave);

                //                _moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name);
                _moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name, AssemblyNameFileName, true);
            }

            public TypeBuilder DefineType(string typeName, Type parentType)
            {
                return _moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Sealed, parentType,null);
            }

            public void Save()
            {
//                _assemblyBuilder.Save(AssemblyNameFileName);
            }
        }
    }
}
