namespace TinyMapper.Mappers
{
    internal abstract class ObjectTypeMapper
    {
        public const string CreateTargetInstanceMethodName = "CreateTargetInstanceCore";
        public const string MapMembersMethodName = "MapMembersCore";

        public object CreateTargetInstance()
        {
            return CreateTargetInstanceCore();
        }

        public object MapMembers(object source, object target)
        {
            return MapMembersCore(source, target);
        }

        protected abstract object CreateTargetInstanceCore();
        protected abstract object MapMembersCore(object source, object target);
    }
}
