namespace TinyMapper.Builders.Assemblies.Types
{
    public abstract class ObjectTypeBuilder
    {
        /// <summary>
        /// public object CreateTargetInstance().
        /// </summary>
        public const string CreateTargetInstanceMethodName = "CreateTargetInstanceCore";

        /// <summary>
        /// public object MapMembers(object source, object target)
        /// </summary>
        public const string MapMembersMethodName = "MapMembersCore";

        public object MapMembers(object source, object target = null)
        {
            if (target == null)
            {
                target = CreateTargetInstanceCore();
            }

            var result = MapMembersCore(source, target);
            return result;
        }

        internal abstract object CreateTargetInstanceCore();
        internal abstract object MapMembersCore(object source, object target);
    }
}
