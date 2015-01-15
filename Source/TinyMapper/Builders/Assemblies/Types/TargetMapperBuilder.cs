using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TinyMapper.Builders.Assemblies.Types.Methods;

namespace TinyMapper.Builders.Assemblies.Types
{
    internal sealed class TargetMapperBuilder
    {
        private readonly IDynamicAssembly _assembly;

        public TargetMapperBuilder(IDynamicAssembly assembly)
        {
            _assembly = assembly;
        }

        public Mapper Build(Type sourceType, Type targetType)
        {
            string mapperTypeName = MapperTypeNameBuilder.Build(sourceType, targetType);
            TypeBuilder typeBuilder = _assembly.DefineType(mapperTypeName, typeof(Mapper));

            var methodBuilders = new List<EmitMethodBuilder>
            {
                new CreateInstanceMethodBuilder(sourceType, targetType, typeBuilder),
                new MapMembersMethodBuilder(sourceType, targetType, typeBuilder),
            };
            methodBuilders.ForEach(x => x.Build());

            Type type = typeBuilder.CreateType();
            var result = (Mapper)Activator.CreateInstance(type);
            return result;
        }
    }
}
