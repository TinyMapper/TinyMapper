using System;
using System.Reflection.Emit;
using TinyMapper.Engines.Builders.Methods;
using TinyMapper.Mappers;

namespace TinyMapper.Engines.Builders
{
    internal sealed class TargetTypeBuilder
    {
        private readonly IDynamicAssembly _assembly;

        public TargetTypeBuilder(IDynamicAssembly assembly)
        {
            _assembly = assembly;
        }

        public void Build(Type sourceType, Type targetType)
        {
            TypeBuilder typeBuilder = _assembly.DefineType("Test1", typeof(MarkerTypeMapper));
            var createInstance = new CreateInstanceMethodBuilder(targetType, typeBuilder);
            createInstance.Build();
        }
    }
}
