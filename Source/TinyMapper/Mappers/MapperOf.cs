namespace TinyMappers.Mappers
{
    internal abstract class MapperOf<TSource, TTarget> : Mapper
    {
        internal override object MapCore(object source, object target)
        {
            return MapCore((TSource)source, (TTarget)target);
        }

        internal abstract TTarget MapCore(TSource source, TTarget target);
    }
}
