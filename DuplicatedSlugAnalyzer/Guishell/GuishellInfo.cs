namespace DuplicatedSlugAnalyzer.Guishell
{
	public class GuishellInfo
	{
		public string GuishellBaseUrl { get; }
		public string ApplicationName { get; }
		public string GuishellSecret { get; }

		public GuishellInfo(string guishellBaseUrl, string applicationName, string guishellSecret)
		{
			this.GuishellBaseUrl = guishellBaseUrl;
			this.ApplicationName = applicationName;
			this.GuishellSecret = guishellSecret;
		}
	}
}
