using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nelibur.ObjectMapper.Mappers
{
	/// <summary>
	/// Exception during the mapping stage
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
