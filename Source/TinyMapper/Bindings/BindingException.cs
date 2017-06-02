using System;
using System.Runtime.Serialization;

namespace Nelibur.ObjectMapper.Bindings
{
    /// <summary>
    ///     Exception occurred during Binding
    /// </summary>
    public class BindingException : TinyMapperException
    {
        public BindingException()
        {
        }

        public BindingException(string message) : base(message)
        {
        }

        public BindingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BindingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
