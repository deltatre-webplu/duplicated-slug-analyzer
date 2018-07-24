using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deltatre.Utils.Extensions.Enumerable;
using DuplicatedSlugAnalyzer.Distribution;
using DuplicatedSlugAnalyzer.Forge;
using DuplicatedSlugAnalyzer.Guishell;
using DuplicatedSlugAnalyzer.Mongodb;
using DuplicatedSlugAnalyzer.Report;
using Microsoft.Extensions.Configuration;
using static System.Console;
using static DuplicatedSlugAnalyzer.Guishell.GuishellHelpers;
using static DuplicatedSlugAnalyzer.Report.JsonHelpers;
using static DuplicatedSlugAnalyzer.Report.ReportHelpers;
using static DuplicatedSlugAnalyzer.Utils.MappingHelpers;

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
		private const int BatchSize = 20;

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

			var mongodbFactory = CreateMongodbFactory(guishellAppConfiguration);
			var duplicateSlugFinder = new DuplicateSlugsFinder(mongodbFactory.PublishedEntitiesCollection);
			var entityCodeToDistributionCode =
				GetEntityCodeToDistributionCodeMap(guishellAppConfiguration.CustomEntitiesConfiguration.Definitions);
			var distributionCollectionFactory = new DistributionCollectionFactory(
				mongodbFactory.DistributionDatabase, 
				entityCodeToDistributionCode);
			var publishedEntityFinder = new PublishedEntityFinder(distributionCollectionFactory);

			WriteLine("\nQuerying backoffice database to get all duplicated slugs for published entities (this could take a long time)...");
			var duplicateSlugsInfos = (await duplicateSlugFinder
				.GetDuplicateSlugsInfoAsync()
				.ConfigureAwait(false)).ToArray();
			WriteLine($"\nFound {duplicateSlugsInfos.Length} duplicated slug reservation keys.");

			WriteLine($"\nCreating reports for duplicate slugs (this could take a long time)...");
			var duplicateSlugsReports = await CreateDuplicateSlugReportsAsync(
					duplicateSlugsInfos, 
					publishedEntityFinder).ConfigureAwait(false);

			WriteLine($"\nPreparing report '{ReportFileName}'. Report will be saved under the folder '{ReportDirectoryName}' which is located at the executable file level.");
			await CreateJsonReportAsync(
				duplicateSlugsReports, 
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

		private static MongodbFactory CreateMongodbFactory(ApplicationConfiguration configuration)
		{
			var backendDbConnString = configuration.BackEndStoreConfiguration.ConnectionString;
			var distributionDbConnString = configuration.DistributionStoreConfiguration.ConnectionString;
			return new MongodbFactory(backendDbConnString, distributionDbConnString);
		}

		private static async Task<IEnumerable<DuplicateSlugReport>> CreateDuplicateSlugReportsAsync(
			IEnumerable<DuplicateSlugInfo> infos,
			PublishedEntityFinder finder)
		{
			var result = new List<DuplicateSlugReport>();

			foreach (var batch in infos.SplitInBatches(BatchSize))
			{
				var reports = await ProcessBatchAsync(batch, finder).ConfigureAwait(false);
				result.AddRange(reports);
			}

			return result;
		}

		private static async Task<IEnumerable<DuplicateSlugReport>> ProcessBatchAsync(
			IEnumerable<DuplicateSlugInfo> batch, 
			PublishedEntityFinder finder)
		{
			var tasks = batch
				.Select(async info => await CreateReportAsync(info, finder).ConfigureAwait(false));

			var reports = await Task.WhenAll(tasks).ConfigureAwait(false);
			return reports;
		}
	}
}
