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
