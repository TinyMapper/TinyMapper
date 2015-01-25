using System.Collections.Generic;

namespace TinyMapper.Mappers
{
    internal abstract class Mapper
    {
        public const string MapMethodName = "Map";
        protected Mapper[] _mappers;

        public void AddMapper(List<Mapper> mappers)
        {
            _mappers = mappers.ToArray();
        }

        public object Map(object source, object target = null)
        {
            return MapCore(source, target);
        }

        internal abstract object MapCore(object source, object target);
    }
}
