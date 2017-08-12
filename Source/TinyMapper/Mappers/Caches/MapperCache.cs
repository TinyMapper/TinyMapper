using System;
using System.Collections.Generic;
using System.Linq;
using Nelibur.ObjectMapper.Core.DataStructures;
using Nelibur.ObjectMapper.Core.Extensions;

namespace Nelibur.ObjectMapper.Mappers.Caches
{
    internal sealed class MapperCache
    {
        private readonly Dictionary<TypePair, MapperCacheItem> _cache = new Dictionary<TypePair, MapperCacheItem>();

        public bool IsEmpty => _cache.Count == 0;

        public List<Mapper> Mappers
        {
            get
            {
                return _cache.Values
                             .OrderBy(x => x.Id)
                             .ConvertAll(x => x.Mapper);
            }
        }

        public MapperCacheItem Add(TypePair key, Mapper value)
        {
            MapperCacheItem result;
            if (_cache.TryGetValue(key, out result))
            {
                return result;
            }
            result = new MapperCacheItem
            {
                Id = GetId(),
                Mapper = value
            };
            _cache[key] = result;
            return result;
        }

        private int GetId()
        {
            return _cache.Count;
        }
    }
}
