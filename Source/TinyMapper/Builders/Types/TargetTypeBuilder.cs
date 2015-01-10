using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TinyMapper.Builders.Assemblies;
using TinyMapper.Builders.Types.Members;
using TinyMapper.Builders.Types.Methods;

namespace TinyMapper.Builders.Types
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

            Type type = typeBuilder.CreateType();
            var t = (TargetTypeMarker)Activator.CreateInstance(type);

            var t1 = new MemberSelector();
            t1.GetMappingItems(new HashSet<MemberSelector.TypesPair>(), sourceType, targetType, null, null);
        }
    }
}
