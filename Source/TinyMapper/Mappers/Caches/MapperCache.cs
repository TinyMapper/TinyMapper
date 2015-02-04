using System.Collections.Generic;
using System.Linq;
using Nelibur.Mapper.Core.DataStructures;

namespace Nelibur.Mapper.Mappers.Caches
{
    internal sealed class MapperCache
    {
        private readonly Dictionary<TypePair, MapperCacheItem> _cache = new Dictionary<TypePair, MapperCacheItem>();

        public bool IsEmpty
        {
            get { return _cache.Count == 0; }
        }

        public List<Mapper> Mappers
        {
            get
            {
                return _cache.Values
                             .OrderBy(x => x.Id)
                             .Select(x => x.Mapper)
                             .ToList();
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
