using TinyMapper.Builders;

namespace TinyMapper
{
    internal sealed class ObjectMapper
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
