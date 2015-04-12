using System;

namespace Nelibur.ObjectMapper.Mappers.Types.Custom
{
    internal sealed class CustomTypeMapper : Mapper
    {
        private readonly Func<object, object> _converter;

        public CustomTypeMapper(Func<object, object> converter)
        {
            _converter = converter;
        }

        protected override object MapCore(object source, object target)
        {
            if (_converter == null)
            {
                return source;
            }
            if (source == null)
            {
                return target;
            }
            return _converter(source);
        }
    }
}
