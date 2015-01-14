using System;
using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.Builders.Assemblies.Types;

namespace TinyMapper.Builders.Assemblies
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
            private readonly object _locker = new object();
            private readonly ModuleBuilder _moduleBuilder;
            private readonly TargetTypeBuilder _targetTypeBuilder;

            public DynamicAssembly()
            {
                var assemblyName = new AssemblyName(AssemblyName);
                _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

                //                // Mark generated code as debuggable. 
                //                // See http://blogs.msdn.com/rmbyers/archive/2005/06/26/432922.aspx for explanation.
                //                Type daType = typeof(DebuggableAttribute);
                //                ConstructorInfo daCtor = daType.GetConstructor(new Type[] { typeof(DebuggableAttribute.DebuggingModes) });
                //                var daBuilder = new CustomAttributeBuilder(daCtor, new object[]
                //                {
                //                    DebuggableAttribute.DebuggingModes.DisableOptimizations |
                //                    DebuggableAttribute.DebuggingModes.Default
                //                });
                //                _assemblyBuilder.SetCustomAttribute(daBuilder);

                _moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name, AssemblyNameFileName, true);

                //                ISymbolDocumentWriter doc = _moduleBuilder.DefineDocument(@"Source.txt", Guid.Empty, Guid.Empty, Guid.Empty);

                _targetTypeBuilder = new TargetTypeBuilder(this);
            }

            public TypeBuilder DefineType(string typeName, Type parentType)
            {
                lock (_locker)
                {
                    return _moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Sealed, parentType);
                }
            }

            public TargetTypeBuilder GetTypeBuilder()
            {
                return _targetTypeBuilder;
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
