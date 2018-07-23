using System;
using System.Runtime.Serialization;

namespace DuplicatedSlugAnalyzer.Distribution
{
	[Serializable]
	public class DistributionCodeNotFoundException : Exception
	{
		public DistributionCodeNotFoundException()
		{
		}

		public DistributionCodeNotFoundException(
			string message) : base(message)
		{
		}

		public DistributionCodeNotFoundException(
			string message,
			Exception innerException) : base(message, innerException)
		{
		}

		protected DistributionCodeNotFoundException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}
