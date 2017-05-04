using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nelibur.ObjectMapper.Core.Extensions;

namespace Nelibur.ObjectMapper.Mappers.Collections
{
    internal abstract class CollectionMapper<TSource, TTarget> : MapperOf<TSource, TTarget> where TTarget : class
    {
        protected virtual object ConvertItem(object item)
        {
            throw new NotImplementedException();
        }

        protected virtual object ConvertItemKey(object item)
        {
            throw new NotImplementedException();
        }

        protected virtual TTarget DictionaryToDictionary(IEnumerable source)
        {
            throw new NotImplementedException();
        }

        protected Dictionary<TTargetKey, TTargetValue> DictionaryToDictionaryTemplate<TSourceKey, TSourceValue, TTargetKey, TTargetValue>(IEnumerable source)
        {
            var result = new Dictionary<TTargetKey, TTargetValue>();
            foreach (KeyValuePair<TSourceKey, TSourceValue> item in source)
            {
                var key = (TTargetKey)ConvertItemKey(item.Key);
                var value = (TTargetValue)ConvertItem(item.Value);
                result.Add(key, value);
            }
            return result;
        }

        protected virtual TTarget EnumerableToArray(IEnumerable source)
        {
            throw new NotImplementedException();
        }

        protected Array EnumerableToArrayTemplate<TTargetItem>(IEnumerable source)
        {
            var result = new TTargetItem[source.Count()];
            int index = 0;
            foreach (var item in source)
            {
                result[index++] = ((TTargetItem)ConvertItem(item));
            }
            return result;
        }

        protected virtual TTarget EnumerableToList(IEnumerable source)
        {
            throw new NotImplementedException();
        }

        protected virtual TTarget EnumerableToArrayList(IEnumerable source)
        {
            var result = new ArrayList();

            foreach (var item in source)
            {
                result.Add(ConvertItem(item));
            }

            return result as TTarget;
        }

        protected List<TTargetItem> EnumerableToListTemplate<TTargetItem>(IEnumerable source)
        {
            var result = new List<TTargetItem>();
            foreach (var item in source)
            {
                result.Add((TTargetItem)ConvertItem(item));
            }
            return result;
        }

        protected virtual TTarget EnumerableToEnumerable(IEnumerable source)
        {
            IList result = null;
            foreach (var item in source)
            {
                if (result == null)
                {
                    result = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(item.GetType()));
                }

                result.Add(ConvertItem(item));
            }
            return result as TTarget;
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
            else if (typeof(TSource).IsDictionaryOf() && targetType.IsDictionaryOf())
            {
                return DictionaryToDictionary(enumerable);
            }
            else if (targetType == typeof(ArrayList))
            {
                return EnumerableToArrayList(enumerable);
            }
            else if (targetType.IsIEnumerable())
            {
                // Default Case
                return EnumerableToEnumerable(enumerable);
            }

            string errorMessage = string.Format("Not suppoerted From {0} To {1}", typeof(TSource).Name, targetType.Name);
            throw new NotSupportedException(errorMessage);
        }
    }
}
