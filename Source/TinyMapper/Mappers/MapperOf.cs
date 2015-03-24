using System;

namespace Nelibur.ObjectMapper.Mappers
{
    internal abstract class MapperOf<TSource, TTarget> : Mapper
    {
        protected override object MapCore(object source, object target)
        {
            if (source == null)
            {
                return default(TTarget);
            }
            return MapCore((TSource)source, (TTarget)target);
        }

        protected abstract TTarget MapCore(TSource source, TTarget target);
    }
}
