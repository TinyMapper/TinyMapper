using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using TinyMapper.DataStructures;
using TinyMapper.Mappers.Types.Members;
using TinyMapper.Reflection;

namespace TinyMapper.Mappers.Builders
{
    internal sealed class CollectionMapperBuilder : MapperBuilder
    {
        private const string MapperNamePrefix = "TinyCollection";
        private const MethodAttributes MethodAttribute = MethodAttributes.Assembly | MethodAttributes.Virtual;

        public CollectionMapperBuilder(IDynamicAssembly dynamicAssembly, TargetMapperBuilder targetMapperBuilder)
            : base(dynamicAssembly, targetMapperBuilder)
        {
        }

        public override bool IsSupported(TypePair typePair)
        {
            return typePair.Target.IsArray
                   || typeof(IEnumerable).IsAssignableFrom(typePair.Target);
        }

        protected override Mapper CreateCore(CompositeMappingMember member)
        {
            string mapperTypeName = GetMapperName();
            TypeBuilder typeBuilder = _assembly.DefineType(mapperTypeName, typeof(CollectionMapper));
            typeBuilder.DefineMethod("CopyToCore", MethodAttribute, typeof(object), new[] { typeof(IEnumerable) });
            throw new NotImplementedException();
        }

        private string GetMapperName()
        {
            return string.Format("{0}_{1}", MapperNamePrefix, Guid.NewGuid().ToString("N"));
        }
    }
}
