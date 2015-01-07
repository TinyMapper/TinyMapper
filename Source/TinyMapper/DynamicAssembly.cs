using System;
using System.Reflection;
using System.Reflection.Emit;

namespace TinyMapper
{
    internal static class DynamicAssembly
    {
        private static readonly AssemblyBuilder _assemblyBuilder;
        private static readonly ModuleBuilder _moduleBuilder;

        static DynamicAssembly()
        {
            var assemblyName = new AssemblyName("TinyMapperAssembly");
            _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name, assemblyName.Name + ".dll", true);
        }

        public static TypeBuilder DefineType(string typeName, Type parentType)
        {
            lock (typeof(DynamicAssembly))
            {
                return _moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Sealed, parentType);
            }
        }

        public static void Save()
        {
            lock (typeof(DynamicAssembly))
            {
                _assemblyBuilder.Save("TinyMapper4Debug.dll");
            }
        }
    }
}
