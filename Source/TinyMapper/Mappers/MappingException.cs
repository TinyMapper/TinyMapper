using System;
using System.Runtime.Serialization;

namespace Nelibur.ObjectMapper.Mappers
{
    /// <summary>
    ///     Exception during the mapping stage
    /// </summary>
    public class MappingException : TinyMapperException
    {
        public MappingException()
        {
        }

        public MappingException(string message) : base(message)
        {
        }

        public MappingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MappingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
