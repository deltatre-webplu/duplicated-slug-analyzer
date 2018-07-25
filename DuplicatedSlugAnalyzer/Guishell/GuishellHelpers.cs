using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;

namespace DuplicatedSlugAnalyzer.Guishell
{
	public static class GuishellHelpers
	{
		private const string JsonMimeType = "application/json";
		private const string AuthorizationHeaderName = "Authorization";
		
		public static async Task<ApplicationConfiguration> GetGuishellAppConfigurationAsync(GuishellInfo info)
		{
			if (info == null)
				throw new ArgumentNullException(nameof(info));

			Log.Debug(
				"Guishell base URL = {GuishellBaseUrl}, Guishell app name = {AppName}, Guishell secret = {GuishellSecret}",
				info.GuishellBaseUrl, info.ApplicationName, info.GuishellSecret);

			var uri = BuildAppConfigurationUri(info.GuishellBaseUrl, info.ApplicationName);
			var authorizationHeaderValue = BuildAuthorizationHeaderValue(info.GuishellSecret);

			Log.Debug(
				"Ready to call Guishell. Url = {Url}, Authorization header = {AuthorizationHeader}",
				uri, authorizationHeaderValue);

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMimeType));
				client.DefaultRequestHeaders.TryAddWithoutValidation(
					AuthorizationHeaderName,
					authorizationHeaderValue);

				var json = await client.GetStringAsync(uri).ConfigureAwait(false);

				Log.Debug("Successfully called Guishell. Ready to deserialize JSON response.");

				var settings = new JsonSerializerSettings
				{
					MissingMemberHandling = MissingMemberHandling.Ignore
				};
				var result = JsonConvert.DeserializeObject<ApplicationConfiguration>(json, settings);

				return result;
			}
		}

		public static string BuildAuthorizationHeaderValue(string guishellSecret) => $"GUISHELL key={guishellSecret}";

		public static Uri BuildAppConfigurationUri(string guishellBaseUrl, string appName)
		{
			var baseUri = new Uri(guishellBaseUrl);
			var relativeUri = $"adminAPI/application/{appName}/config";
			return new Uri(baseUri, relativeUri);
		}
	}
}
