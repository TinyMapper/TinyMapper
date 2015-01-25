using System.Collections.Generic;
using TinyMapper.DataStructures;

namespace TinyMapper.Mappers
{
    internal sealed class MapperCache
    {
        private readonly Dictionary<TypePair, MapperCacheItem> _cache = new Dictionary<TypePair, MapperCacheItem>();

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
