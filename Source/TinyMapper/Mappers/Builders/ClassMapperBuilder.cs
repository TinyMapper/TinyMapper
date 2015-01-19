using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Builders.Methods;
using TinyMapper.Mappers.Types;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers.Builders
{
    internal sealed class ClassMapperBuilder : MapperBuilder
    {
        private const string MapperNamePrefix = "TinyClass";

        public ClassMapperBuilder(IDynamicAssembly dynamicAssembly, TargetMapperBuilder targetMapperBuilder)
            : base(dynamicAssembly, targetMapperBuilder)
        {
        }

        public override bool IsSupported(TypePair typePair)
        {
            return true;
        }

        protected override Mapper CreateCore(MappingType mappingType)
        {
            string mapperTypeName = GetMapperName(mappingType.TypePair);
            TypeBuilder typeBuilder = _assembly.DefineType(mapperTypeName, typeof(Mapper));

            var methodBuilders = new List<EmitMethodBuilder>
            {
                new CreateInstanceMethodBuilder(mappingType, typeBuilder),
                new MapMembersMethodBuilder(mappingType, typeBuilder),
            };
            methodBuilders.ForEach(x => x.Build());

            Type type = typeBuilder.CreateType();
            var result = (Mapper)Activator.CreateInstance(type);
            return result;
        }

        private string GetMapperName(TypePair pair)
        {
            string random = Guid.NewGuid().ToString("N");
            string sourceFullName = pair.Source.FullName;
            string targetFullName = pair.Target.FullName;
            return string.Format("{0}_{1}_{2}_{3}", MapperNamePrefix, sourceFullName, targetFullName, random);
        }
    }
}
