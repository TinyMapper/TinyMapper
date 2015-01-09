using System;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyMapper.Builders
{
    internal class DynamicAssemblyBuilder
    {
        internal const string AssemblyName = "TinyMapper_E028674AFAE9";
        private const string AssemblyNameFileName = AssemblyName + ".dll";
        private static readonly DynamicAssembly _dynamicAssembly = new DynamicAssembly();

        public static IDynamicAssembly Build()
        {
            return _dynamicAssembly;
        }


        private sealed class DynamicAssembly : IDynamicAssembly
        {
            private readonly AssemblyBuilder _assemblyBuilder;
            private readonly object _locker = new object();
            private readonly ModuleBuilder _moduleBuilder;

            public DynamicAssembly()
            {
                var assemblyName = new AssemblyName(AssemblyName);
                _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
                _moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name, AssemblyNameFileName, true);
            }

            public TypeBuilder DefineType(string typeName, Type parentType)
            {
                lock (_locker)
                {
                    return _moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Sealed, parentType);
                }
            }

            public void Save()
            {
                lock (_locker)
                {
                    _assemblyBuilder.Save(AssemblyNameFileName);
                }
            }
        }
    }
}
