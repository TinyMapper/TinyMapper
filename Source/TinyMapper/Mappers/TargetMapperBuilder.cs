using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Builders;
using TinyMapper.Mappers.Builders.Methods;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers
{
    internal sealed class TargetMapperBuilder
    {
        private readonly IDynamicAssembly _assembly;

        public TargetMapperBuilder(IDynamicAssembly assembly)
        {
            _assembly = assembly;
        }

        public Mapper Build(TypePair typePair)
        {
            string mapperTypeName = MapperTypeNameBuilder.Build(typePair);
            TypeBuilder typeBuilder = _assembly.DefineType(mapperTypeName, typeof(Mapper));

            var methodBuilders = new List<EmitMethodBuilder>
            {
                new CreateInstanceMethodBuilder(typePair, typeBuilder),
                new MapMembersMethodBuilder(typePair, typeBuilder),
            };
            methodBuilders.ForEach(x => x.Build());

            Type type = typeBuilder.CreateType();
            var result = (Mapper)Activator.CreateInstance(type);
            return result;
        }
    }
}
