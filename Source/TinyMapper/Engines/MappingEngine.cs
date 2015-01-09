using TinyMapper.Engines.Builders;

namespace TinyMapper.Engines
{
    internal sealed class MappingEngine
    {
        public static void CreateMapper<TSource, TTarget>()
        {
            IDynamicAssembly assembly = DynamicAssemblyBuilder.Build();
            var targetTypeBuilder = new TargetTypeBuilder(assembly);
            targetTypeBuilder.Build(typeof(TSource), typeof(TTarget));

            assembly.Save();
        }
    }
}
