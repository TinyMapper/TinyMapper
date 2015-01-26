using System.Collections.Generic;
using System.Reflection;

namespace TinyMappers.Mappers
{
    internal abstract class Mapper
    {
        public const string MapMethodName = "Map";
        public const string MappersFieldName = "_mappers";
        protected const MethodAttributes OverrideProtected = MethodAttributes.Family | MethodAttributes.Virtual;
        protected Mapper[] _mappers;

        public void AddMappers(List<Mapper> mappers)
        {
            _mappers = mappers.ToArray();
        }

        public TTarget Map<TSource, TTarget>(TSource source, TTarget target = default(TTarget))
        {
            return MapCore(source, target);
        }

        internal abstract TTarget MapCore<TSource, TTarget>(TSource source, TTarget target);
    }
}
