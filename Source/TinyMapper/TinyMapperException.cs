using System;
using System.Runtime.Serialization;

namespace Nelibur.ObjectMapper
{
    /// <summary>
    ///     Exception during mapping or binding
    /// </summary>
    public class TinyMapperException : Exception
    {
        public TinyMapperException()
        {
        }

        public TinyMapperException(string message) : base(message)
        {
        }

        public TinyMapperException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TinyMapperException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
