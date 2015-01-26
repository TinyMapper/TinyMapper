using System;
using System.Collections.Generic;
using System.Reflection;

namespace TinyMapper.Mappers
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

        public object Map(object source, object target = null)
        {
            return MapCore(source, target);
        }

        internal abstract object MapCore(object source, object target);
    }
}
