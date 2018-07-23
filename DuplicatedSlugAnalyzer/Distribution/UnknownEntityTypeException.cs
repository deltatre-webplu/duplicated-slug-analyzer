using System;
using System.Runtime.Serialization;

namespace DuplicatedSlugAnalyzer.Distribution
{
	public class UnknownEntityTypeException : Exception
	{
		public UnknownEntityTypeException()
		{
		}

		public UnknownEntityTypeException(string message) : base(message)
		{
		}

		public UnknownEntityTypeException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected UnknownEntityTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
