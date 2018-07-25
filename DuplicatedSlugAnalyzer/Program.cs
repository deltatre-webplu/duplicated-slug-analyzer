using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Deltatre.Utils.Extensions.Enumerable;
using DuplicatedSlugAnalyzer.Distribution;
using DuplicatedSlugAnalyzer.Forge;
using DuplicatedSlugAnalyzer.Guishell;
using DuplicatedSlugAnalyzer.Report;
using Microsoft.Extensions.Configuration;
using Serilog;
using static System.Console;
using static DuplicatedSlugAnalyzer.Guishell.GuishellHelpers;
using static DuplicatedSlugAnalyzer.Report.JsonReport;
using static DuplicatedSlugAnalyzer.Report.ReportHelpers;
using static DuplicatedSlugAnalyzer.Constants;
using static DuplicatedSlugAnalyzer.Utils.Builders;
using static DuplicatedSlugAnalyzer.Logging.Serilog;
using static DuplicatedSlugAnalyzer.Utils.Configuration;

namespace DuplicatedSlugAnalyzer
{
	public class Program
	{
		private static void Main(string[] args)
		{
			var config = GetConfiguration(args);

			BootstrapLogger(config);

			Log.Debug("Start processing");

			try
			{
				RunAsync(config).Wait();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "An error occurred");
			}

			Log.Debug("End processing");

			Log.CloseAndFlush();

			ReadLine();
		}

		private static async Task RunAsync(IConfiguration config)
		{
			WriteLine("Welcome to Webplu Duplicate Slugs Analyzer. Press enter to start.");
			ReadLine();

			Log.Information("Reading configuration to get guishell info");
			var guishellBaseUrl = config[GuishellBaseUrlConfigKey];
			var applicationName = config[ApplicationNameConfigKey];
			var guishellSecret = config[GuishellSecretConfigKey];

			Log.Information("Calling guishell to get Forge configuration");
			var guishellInfo = new GuishellInfo(guishellBaseUrl, applicationName, guishellSecret);
			var guishellAppConfiguration = await GetGuishellAppConfigurationAsync(guishellInfo)
				.ConfigureAwait(false);

			var mongodbFactory = CreateMongodbFactory(guishellAppConfiguration);
			var duplicateSlugFinder = CreateDuplicateSlugFinder(mongodbFactory.BackendDatabase);
			var publishedEntityFinder = CreatePublishedEntityFinder(
				mongodbFactory.DistributionDatabase, 
				guishellAppConfiguration);

			Log.Information("Querying backoffice database to get duplicated slugs (this could take a long time)");
			var duplicateSlugsInfos = (await duplicateSlugFinder
				.GetDuplicateSlugsInfoAsync()
				.ConfigureAwait(false)).ToArray();
			Log.Information($"Found {duplicateSlugsInfos.Length} duplicated slug reservation keys.");

			Log.Information("Creating reports for duplicate slugs (this could take a long time)");
			var duplicateSlugsReports = await CreateDuplicateSlugReportsAsync(
					duplicateSlugsInfos, 
					publishedEntityFinder).ConfigureAwait(false);

			var reportDirectoryPath = GetJsonReportDirectoryPath(config);
			Log.Information($"Writing report '{ReportFileName}' under folder '{reportDirectoryPath}'");
			await CreateJsonReportAsync(
				duplicateSlugsReports, 
				reportDirectoryPath).ConfigureAwait(false);

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

		private static void BootstrapLogger(IConfiguration configuration)
		{
			var logsDirectoryPath = GetLogsDirectoryPath(configuration);
			Log.Logger = CreateLogger(logsDirectoryPath);
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

		private static string GetJsonReportDirectoryPath(IConfiguration configuration) =>
			ReadSettingFromConfiguration(configuration, ReportDirectoryPathConfigKey, GetDefaultReportDirectoryPath);

		private static string GetLogsDirectoryPath(IConfiguration configuration) =>
			ReadSettingFromConfiguration(configuration, LogsDirectoryPathConfigKey, GetDefaultLogsDirectoryPath);
	}
}
