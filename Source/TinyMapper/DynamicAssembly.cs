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
            string name = string.Format("TinyMapper_{0}", Guid.NewGuid().ToString("N"));
            var assemblyName = new AssemblyName(name);

            _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name, true);
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
