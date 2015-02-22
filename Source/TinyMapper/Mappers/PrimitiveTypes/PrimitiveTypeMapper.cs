using System;

namespace Nelibur.ObjectMapper.Mappers.PrimitiveTypes
{
    internal sealed class PrimitiveTypeMapper : Mapper
    {
        private readonly Func<object, object> _converter;

        public PrimitiveTypeMapper(Func<object, object> converter)
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
