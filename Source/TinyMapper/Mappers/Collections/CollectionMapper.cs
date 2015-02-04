using System;
using System.Collections;
using System.Collections.Generic;

namespace Nelibur.Mapper.Mappers.Collections
{
    internal abstract class CollectionMapper<TSource, TTarget> : MapperOf<TSource, TTarget>
    {
        protected virtual object ConvertItem(object item)
        {
            throw new NotImplementedException();
        }

        protected virtual TTarget EnumerableToList(IEnumerable value)
        {
            throw new NotImplementedException();
        }

        protected List<TTargetItem> EnumerableToListTemplate<TTargetItem>(IEnumerable source)
        {
            var result = new List<TTargetItem>();
            foreach (object item in source)
            {
                result.Add((TTargetItem)ConvertItem(item));
            }
            return result;
        }

        protected override TTarget MapCore(TSource source, TTarget target)
        {
            return EnumerableToList((IEnumerable)source);
        }
    }
}
