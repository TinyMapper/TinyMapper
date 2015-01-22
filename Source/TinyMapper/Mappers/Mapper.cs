using System.Collections.Generic;
using TinyMapper.Mappers.Types.Members;
using TinyMapper.TypeConverters;

namespace TinyMapper.Mappers
{
    internal abstract class Mapper
    {
        /// <summary>
        ///     public object CreateTargetInstance().
        /// </summary>
        public const string CreateTargetInstanceMethodName = "CreateTargetInstanceCore";

        /// <summary>
        ///     public object MapMembers(object source, object target)
        /// </summary>
        public const string MapMembersMethodName = "MapMembersCore";

        protected readonly Dictionary<MappingMember, ITypeConverter> _converters;

        protected Mapper()
        {
            _converters = new Dictionary<MappingMember, ITypeConverter>();
        }

        public object MapMembers(object source, object target = null)
        {
            if (target == null)
            {
                target = CreateTargetInstanceCore();
            }

            object result = MapMembersCore(source, target);
            return result;
        }

        internal abstract object CreateTargetInstanceCore();
        internal abstract object MapMembersCore(object source, object target);
    }
}
