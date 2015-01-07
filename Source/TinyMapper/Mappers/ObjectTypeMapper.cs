namespace TinyMapper.Mappers
{
    internal abstract class ObjectTypeMapper
    {
        public object CreateTargetInstance()
        {
            return CreateTargetInstanceCore();
        }

        protected abstract object CreateTargetInstanceCore();
    }
}
