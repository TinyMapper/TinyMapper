namespace TinyMapper.Mappers
{
    internal abstract class Mapper
    {
        public object Map(object source, object target = null)
        {
            return MapCore(source, target);
        }

        internal abstract object MapCore(object source, object target);
    }
}
