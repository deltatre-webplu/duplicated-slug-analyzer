using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DuplicatedSlugAnalyzer.Guishell
{
	public static class GuishellHelpers
	{
		private const string JsonMimeType = "application/json";
		private const string AuthorizationHeaderName = "Authorization";
		
		public static async Task<ApplicationConfiguration> GetAppConfigurationAsync(
			GuishellApplicationInfo applicationInfo)
		{
			if (applicationInfo == null)
				throw new ArgumentNullException(nameof(applicationInfo));

			var uri = BuildAppConfigurationUri(
				applicationInfo.GuishellBaseUrl, 
				applicationInfo.ApplicationName);

			var authorizationHeaderValue = BuildAuthorizationHeaderValue(applicationInfo.GuishellSecret);

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMimeType));
				client.DefaultRequestHeaders.TryAddWithoutValidation(
					AuthorizationHeaderName,
					authorizationHeaderValue);

				var json = await client.GetStringAsync(uri).ConfigureAwait(false);

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
