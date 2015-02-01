using System.Collections.Generic;

namespace TinyMappers.Mappers
{
    internal abstract class Mapper
    {
        public const string MapMethodName = "Map";
        public const string MappersFieldName = "_mappers";
        protected Mapper[] _mappers;

        public void AddMappers(List<Mapper> mappers)
        {
            _mappers = mappers.ToArray();
        }

        public object Map(object source, object target = null)
        {
            return MapCore(source, target);
        }

        protected abstract object MapCore(object source, object target);
    }
}
