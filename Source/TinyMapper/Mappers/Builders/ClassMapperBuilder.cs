using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Builders.Methods;
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

        protected override Mapper CreateCore(TypePair typePair)
        {
            string mapperTypeName = GetMapperName(typePair);
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

        private static string GetFullName(Type type)
        {
            return type == null ? "Empty" : type.FullName;
        }

        private string GetMapperName(TypePair pair)
        {
            string random = Guid.NewGuid().ToString("N");
            string sourceFullName = GetFullName(pair.Source);
            string targetFullName = GetFullName(pair.Target);
            return string.Format("{0}_{1}_{2}_{3}", MapperNamePrefix, sourceFullName, targetFullName, random);
        }
    }
}
