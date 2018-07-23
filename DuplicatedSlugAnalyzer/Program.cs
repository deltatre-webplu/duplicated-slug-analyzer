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
			RunAsync(args).Wait();
			ReadLine();
		}

		private static async Task RunAsync(string[] args)
		{
			WriteLine("Reading configuration to get guishell info...");

			var config = GetConfiguration(args);

			var guishellBaseUrl = config[GuishellBaseUrlConfigKey];
			var applicationName = config[ApplicationNameConfigKey];
			var guishellSecret = config[GuishellSecretConfigKey];

			WriteLine("Calling guishell to get Forge configuration...");

			var guishellInfo = new GuishellInfo(guishellBaseUrl, applicationName, guishellSecret);
			var guishellAppConfiguration = await GetGuishellAppConfigurationAsync(guishellInfo)
				.ConfigureAwait(false);

			WriteLine("Querying backoffice database to get all duplicated slugs for published entities...");

			var mongodbConnString = guishellAppConfiguration.BackEndStoreConfiguration.ConnectionString;
			var duplicateSlugFinder = DuplicateSlugsFinderFactory.Create(mongodbConnString);
			var duplicateSlugsInfos = await duplicateSlugFinder
				.GetDuplicateSlugsInfoAsync()
				.ConfigureAwait(false);

			WriteLine("Preparing JSON report...");

			await CreateJsonReportAsync(
				duplicateSlugsInfos, 
				ReportFileName, 
				ReportDirectoryName).ConfigureAwait(false);

			WriteLine("All done");
		}

		private static IConfiguration GetConfiguration(string[] args)
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile(ConfigurationFileName, true, true)
				.AddEnvironmentVariables()
				.AddCommandLine(args)
				.Build();

			return config;
		}
	}
}
