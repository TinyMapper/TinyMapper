using System;
using System.Collections.Generic;
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
            TypeBuilder typeBuilder = _assembly.DefineType(Guid.NewGuid().ToString(), typeof(MarkerTypeMapper));

            var methodBuilders = new List<EmitMethodBuilder>
            {
                new CreateInstanceMethodBuilder(sourceType, targetType, typeBuilder),
                new MapMembersMethodBuilder(sourceType, targetType, typeBuilder),
            };
            methodBuilders.ForEach(x => x.Build());

            Type type = typeBuilder.CreateType();
            var t = (MarkerTypeMapper)Activator.CreateInstance(type);
        }
    }
}
