using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Deltatre.Utils.Extensions.Dictionary;
using DuplicatedSlugAnalyzer.Forge;
using DuplicatedSlugAnalyzer.Guishell;
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
			try
			{
				RunAsync(args).Wait();
			}
			catch (Exception e)
			{
				WriteLine(e);
			}

			ReadLine();
		}

		private static async Task RunAsync(string[] args)
		{
			WriteLine("Welcome to Webplu Duplicate Slugs Analyzer. Press enter to start.");
			ReadLine();

			WriteLine("\nReading configuration to get guishell info...");
			var config = GetConfiguration(args);
			var guishellBaseUrl = config[GuishellBaseUrlConfigKey];
			var applicationName = config[ApplicationNameConfigKey];
			var guishellSecret = config[GuishellSecretConfigKey];

			WriteLine("\nCalling guishell to get Forge configuration...");
			var guishellInfo = new GuishellInfo(guishellBaseUrl, applicationName, guishellSecret);
			var guishellAppConfiguration = await GetGuishellAppConfigurationAsync(guishellInfo)
				.ConfigureAwait(false);

			WriteLine("\nQuerying backoffice database to get all duplicated slugs for published entities (this could take a long time)...");
			var mongodbConnString = guishellAppConfiguration.BackEndStoreConfiguration.ConnectionString;
			var duplicateSlugFinder = DuplicateSlugsFinder.Create(mongodbConnString);
			var duplicateSlugsInfos = (await duplicateSlugFinder
				.GetDuplicateSlugsInfoAsync()
				.ConfigureAwait(false)).ToArray();
			WriteLine($"\nFound {duplicateSlugsInfos.Length} duplicated slug reservation keys.");

			WriteLine($"\nPreparing report '{ReportFileName}'. Report will be saved under the folder '{ReportDirectoryName}' which is located at the executable file level.");
			await CreateJsonReportAsync(
				duplicateSlugsInfos, 
				ReportFileName, 
				ReportDirectoryName).ConfigureAwait(false);

			WriteLine("\nExecution successfully completed. Press enter to close.");
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

		private static ReadOnlyDictionary<string, string> GetEntityCodeToDistributionCodeMap(
			IEnumerable<CustomEntity> customEntities) => 
			customEntities
				.ToDictionary(
					ce => ce.Code, 
					ce => ce.DistributionCode)
				.AsReadOnly();
	}
}
