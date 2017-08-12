using System;
#if !COREFX
using System.Runtime.Serialization;
#endif

namespace Nelibur.ObjectMapper
{
    /// <summary>
    ///     Exception during mapping or binding
    /// </summary>
#if !COREFX
    [Serializable]
#endif
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
#if !COREFX
        protected TinyMapperException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}
