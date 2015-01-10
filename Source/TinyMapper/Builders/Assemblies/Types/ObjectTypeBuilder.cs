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

        public object CreateTargetInstance()
        {
            return CreateTargetInstanceCore();
        }

        public object MapMembers(object source, object target)
        {
            return MapMembersCore(source, target);
        }

        internal abstract object CreateTargetInstanceCore();
        internal abstract object MapMembersCore(object source, object target);
    }
}
