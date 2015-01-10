using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TinyMapper.Builders.Assemblies.Types.Members;
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

        public void Build(Type sourceType, Type targetType)
        {
            string mapperTypeName = MapperTypeNameBuilder.Build(sourceType, targetType);
            TypeBuilder typeBuilder = _assembly.DefineType(mapperTypeName, typeof(TargetTypeMarker));

            var methodBuilders = new List<EmitMethodBuilder>
            {
                new CreateInstanceMethodBuilder(sourceType, targetType, typeBuilder),
                new MapMembersMethodBuilder(sourceType, targetType, typeBuilder),
            };
            methodBuilders.ForEach(x => x.Build());

            var memberSelector = new MemberSelector();
            List<MappingMember> mappingMembers = memberSelector.GetMappingMembers(sourceType, targetType);

            Type type = typeBuilder.CreateType();
            var t = (TargetTypeMarker)Activator.CreateInstance(type);
        }
    }
}
