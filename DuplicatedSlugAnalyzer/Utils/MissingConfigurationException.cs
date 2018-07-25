﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DuplicatedSlugAnalyzer.Utils
{
	public class MissingConfigurationException : Exception
	{
		public MissingConfigurationException()
		{
		}

		public MissingConfigurationException(string message) : base(message)
		{
		}

		public MissingConfigurationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected MissingConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
