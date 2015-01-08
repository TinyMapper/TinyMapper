using System;
using TinyMapper.Engines.Builders;

namespace TinyMapper.Engines
{
    internal sealed class MappingEngine
    {
        public void CreateMapper(Type sourceType, Type targetType)
        {
            IDynamicAssembly assembly = DynamicAssemblyBuilder.Build(); //.DefineType("Test1", typeof(MarkerTypeMapper));
            var targetTypeBuilder = new TargetTypeBuilder(assembly);
            targetTypeBuilder.Build(sourceType, targetType);
        }
    }
}
