using System;

namespace Nelibur.ObjectMapper.Mappers.Types.Custom
{
    internal sealed class CustomMemberMapper : Mapper
    {
        private readonly Func<object, object> _converter;

        public CustomMemberMapper(Func<object, object> converter)
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
