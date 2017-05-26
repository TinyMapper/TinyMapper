using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nelibur.ObjectMapper
{
	/// <summary>
	/// Exception during mapping or binding
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
