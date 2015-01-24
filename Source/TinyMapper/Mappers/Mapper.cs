using System.Collections.Generic;
using TinyMapper.Mappers.Collections;

namespace TinyMapper.Mappers
{
    internal abstract class Mapper
    {
        /// <summary>
        ///     public object CreateTargetInstance().
        /// </summary>
        public const string CreateTargetInstanceMethodName = "CreateTargetInstanceCore";

        /// <summary>
        ///     public object Map(object source, object target)
        /// </summary>
        public const string MapMembersMethodName = "MapCore";

        protected readonly List<CollectionMapper> _mappers = new List<CollectionMapper>();

        public void AddMapper(CollectionMapper mapper)
        {
            _mappers.Add(mapper);
        }

        public object Map(object source, object target = null)
        {
            if (target == null)
            {
                target = CreateTargetInstanceCore();
            }

            object result = MapCore(source, target);
            return result;
        }

        internal abstract object CreateTargetInstanceCore();
        internal abstract object MapCore(object source, object target);
    }
}
