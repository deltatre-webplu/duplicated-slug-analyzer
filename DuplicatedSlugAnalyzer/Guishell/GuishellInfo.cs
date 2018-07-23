using System;

namespace DuplicatedSlugAnalyzer.Guishell
{
	public struct GuishellInfo
	{
		public string GuishellBaseUrl { get; }
		public string ApplicationName { get; }
		public string GuishellSecret { get; }

		public GuishellInfo(string guishellBaseUrl, string applicationName, string guishellSecret)
		{
			if(string.IsNullOrWhiteSpace(guishellBaseUrl))
				throw new ArgumentException("Guishell base url cannot be null or white space.", nameof(guishellBaseUrl));

			if (string.IsNullOrWhiteSpace(applicationName))
				throw new ArgumentException("Guishell application name cannot be null or white space.", nameof(applicationName));

			if (string.IsNullOrWhiteSpace(guishellSecret))
				throw new ArgumentException("Guishell secret cannot be null or white space.", nameof(guishellSecret));

			this.GuishellBaseUrl = guishellBaseUrl;
			this.ApplicationName = applicationName;
			this.GuishellSecret = guishellSecret;
		}
	}
}
