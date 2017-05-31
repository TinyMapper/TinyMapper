using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nelibur.ObjectMapper.Bindings
{
	/// <summary>
	/// Exception occurred during Binding
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
