using System;

namespace Nelibur.Mapper.Mappers
{
    internal abstract class MapperOf<TSource, TTarget> : Mapper
    {
        protected override object MapCore(object source, object target)
        {
            return MapCore((TSource)source, (TTarget)target);
        }

        protected abstract TTarget MapCore(TSource source, TTarget target);
    }
}
