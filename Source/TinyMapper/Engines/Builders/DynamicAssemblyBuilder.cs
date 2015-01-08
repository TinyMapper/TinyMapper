using System;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyMapper.Engines.Builders
{
    internal class DynamicAssemblyBuilder
    {
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
                var assemblyName = new AssemblyName("TinyMapperAssembly");
                _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
                _moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name, assemblyName.Name + ".dll", true);
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
                    _assemblyBuilder.Save("TinyMapper4Debug.dll");
                }
            }
        }
    }
}
