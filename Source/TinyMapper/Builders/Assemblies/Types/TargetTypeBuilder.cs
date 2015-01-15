using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TinyMapper.Builders.Assemblies.Types.Methods;

namespace TinyMapper.Builders.Assemblies.Types
{
    internal sealed class TargetTypeBuilder
    {
        private readonly IDynamicAssembly _assembly;

        public TargetTypeBuilder(IDynamicAssembly assembly)
        {
            _assembly = assembly;
        }

        public ObjectTypeBuilder Build(Type sourceType, Type targetType)
        {
            string mapperTypeName = MapperTypeNameBuilder.Build(sourceType, targetType);
            TypeBuilder typeBuilder = _assembly.DefineType(mapperTypeName, typeof(ObjectTypeBuilder));

            var methodBuilders = new List<EmitMethodBuilder>
            {
                new CreateInstanceMethodBuilder(sourceType, targetType, typeBuilder),
                new MapMembersMethodBuilder(sourceType, targetType, typeBuilder),
            };
            methodBuilders.ForEach(x => x.Build());

            Type type = typeBuilder.CreateType();
            var result = (ObjectTypeBuilder)Activator.CreateInstance(type);
            return result;
        }
    }
}
