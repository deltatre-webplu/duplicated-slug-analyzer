using System.Threading.Tasks;
using DuplicatedSlugAnalyzer.Guishell;
using DuplicatedSlugAnalyzer.Mongodb;
using Microsoft.Extensions.Configuration;
using static System.Console;
using static DuplicatedSlugAnalyzer.Guishell.GuishellHelpers;
using static DuplicatedSlugAnalyzer.Report.ReportHelpers;

namespace DuplicatedSlugAnalyzer
{
	public class Program
	{
		private const string ConfigurationFileName = "appsettings.json";
		private const string ReportFileName = "report.json";
		private const string ReportDirectoryName = "Reports";
		private const string GuishellBaseUrlConfigKey = "guishellBaseUrl";
		private const string ApplicationNameConfigKey = "applicationName";
		private const string GuishellSecretConfigKey = "guishellSecret";


		private static void Main(string[] args)
		{
			RunAsync().Wait();
			ReadLine();
		}

		private static async Task RunAsync()
		{
			var config = GetConfiguration();

			var guishellBaseUrl = config[GuishellBaseUrlConfigKey];
			var applicationName = config[ApplicationNameConfigKey];
			var guishellSecret = config[GuishellSecretConfigKey];

			var guishellInfo = new GuishellInfo(guishellBaseUrl, applicationName, guishellSecret);
			var guishellAppConfiguration = await GetGuishellAppConfigurationAsync(guishellInfo)
				.ConfigureAwait(false);

			var mongodbConnString = guishellAppConfiguration.BackEndStoreConfiguration.ConnectionString;
			var duplicateSlugFinder = DuplicateSlugsFinderFactory.Create(mongodbConnString);
			var duplicateSlugsInfos = await duplicateSlugFinder
				.GetDuplicateSlugsInfoAsync()
				.ConfigureAwait(false);

			await CreateJsonReportAsync(
				duplicateSlugsInfos, 
				ReportFileName, 
				ReportDirectoryName).ConfigureAwait(false);
		}

		private static IConfiguration GetConfiguration()
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile(ConfigurationFileName, true, true)
				.Build();

			return config;
		}
	}
}
