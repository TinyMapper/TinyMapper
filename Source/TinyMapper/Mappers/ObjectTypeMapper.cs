namespace TinyMapper.Mappers
{
    internal abstract class ObjectTypeMapper
    {
        public object CreateTargetInstance()
        {
            return CreateTargetInstanceCore();
        }

        public const string CreateTargetInstanceMethodName = "CreateTargetInstanceCore";

        protected abstract object CreateTargetInstanceCore();
    }
}
