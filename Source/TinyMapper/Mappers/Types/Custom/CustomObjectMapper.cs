using System;

namespace Nelibur.ObjectMapper.Mappers.Types.Custom
{
    internal sealed class CustomObjectMapper : Mapper
    {
        private readonly Func<object, object> _converter;

        public CustomObjectMapper(Func<object, object> converter)
        {
            _converter = converter;
        }

        protected override object MapCore(object source, object target)
        {
            if (_converter == null)
            {
                return source;
            }
            return _converter(source);
        }
    }
}
