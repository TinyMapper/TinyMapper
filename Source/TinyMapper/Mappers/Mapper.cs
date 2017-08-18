using System;
using System.Collections.Generic;
using System.Linq;

namespace Nelibur.ObjectMapper.Mappers
{
    internal abstract class Mapper
    {
        public const string MapMethodName = "Map";
        public const string MappersFieldName = "_mappers";
        protected Mapper[] _mappers;

        public void AddMappers(IEnumerable<Mapper> mappers)
        {
            _mappers = mappers.ToArray();
        }

        public void UpdateRootMapper(int mapperId, Mapper mapper)
        {
            if (_mappers == null)
            {
                return;
            }

            for (int i = 0; i < _mappers.Length; i++)
            {
                if (i == mapperId)
                {
                    if (_mappers[i] == null)
                    {
                        _mappers[i] = mapper;
                    }
                    return;
                }
            }
        }

        public object Map(object source, object target = null)
        {
            return MapCore(source, target);
        }

        protected abstract object MapCore(object source, object target);
    }
}
