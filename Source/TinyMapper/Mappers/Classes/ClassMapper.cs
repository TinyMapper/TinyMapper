using System;
using TinyMappers.Nelibur.Sword.Extensions;

namespace TinyMappers.Mappers.Classes
{
    internal abstract class ClassMapper<TSource, TTarget> : MapperOf<TSource, TTarget>
    {
        protected override TTarget MapCore(TSource source, TTarget target)
        {
            if (target.IsNull())
            {
                target = CreateTargetInstance();
            }
            TTarget result = MapClass(source, target);
            return result;
        }

        protected virtual TTarget CreateTargetInstance()
        {
            throw new NotImplementedException();
        }

        protected abstract TTarget MapClass(TSource source, TTarget target);
    }
}
