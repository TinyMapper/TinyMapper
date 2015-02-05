using System;

namespace Nelibur.ObjectMapper.Mappers.Caches
{
    internal sealed class MapperCacheItem
    {
        public int Id { get; set; }
        public Mapper Mapper { get; set; }
    }
}
