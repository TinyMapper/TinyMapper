using System;
using System.Collections;
using System.Collections.Generic;
using Nelibur.ObjectMapper.Core.Extensions;

namespace Nelibur.ObjectMapper.Mappers.Collections
{
    internal abstract class CollectionMapper<TSource, TTarget> : MapperOf<TSource, TTarget>
    {
        protected virtual object ConvertItem(object item)
        {
            throw new NotImplementedException();
        }

        protected virtual TTarget EnumerableToArray(IEnumerable source)
        {
            throw new NotImplementedException();
        }

        protected Array EnumerableToArrayTemplate<TTargetItem>(IEnumerable source)
        {
            var result = new TTargetItem[source.Count()];
            int index = 0;
            foreach (object item in source)
            {
                result[index++] = ((TTargetItem)ConvertItem(item));
            }
            return result;
        }

        protected virtual TTarget EnumerableToList(IEnumerable source)
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
            Type targetType = typeof(TTarget);
            var enumerable = (IEnumerable)source;

            if (targetType.IsListOf())
            {
                return EnumerableToList(enumerable);
            }
            else if (targetType.IsArray)
            {
                return EnumerableToArray(enumerable);
            }
            string errorMessage = string.Format("Not suppoerted From {0} To {1}", typeof(TSource).Name, targetType.Name);
            throw new NotSupportedException(errorMessage);
        }
    }
}
