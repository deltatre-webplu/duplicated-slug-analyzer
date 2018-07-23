namespace DuplicatedSlugAnalyzer.Guishell
{
	public class GuishellApplicationInfo
	{
		public string GuishellBaseUrl { get; }
		public string ApplicationName { get; }
		public string GuishellSecret { get; }

		public GuishellApplicationInfo(string guishellBaseUrl, string applicationName, string guishellSecret)
		{
			this.GuishellBaseUrl = guishellBaseUrl;
			this.ApplicationName = applicationName;
			this.GuishellSecret = guishellSecret;
		}
	}
}
