using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DuplicatedSlugAnalyzer.Mongodb;
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
		private const int MinimumNumberOfCommandLineArgumentsExpected = 3;

		private static void Main(string[] args)
		{
			RunAsync(args).Wait();
			ReadLine();
		}

		private static async Task RunAsync(string[] args)
		{
			var appConfiguration = await GetAppConfigurationAsync(null).ConfigureAwait(false);

			var connString = appConfiguration.BackEndStoreConfiguration.ConnectionString;
			var duplicateSlugFinder = DuplicateSlugsFinderFactory.Create(connString);

			var docs = await duplicateSlugFinder.GetDuplicateSlugsInfoAsync().ConfigureAwait(false);

			await CreateJsonReportAsync(docs, ReportFileName, ReportDirectoryName).ConfigureAwait(false);
		}

		private static (string guishellBaseUrl, string applicationName, string guishellSecret) 
			ExtractGuishellDataFromCommandLineArgs(IReadOnlyList<string> args)
		{
			if(args.Count == 0)
				throw new ArgumentException(
					"No command line arguments provided. Unable to proceed.", 
					nameof(args));

			if(args.Count < MinimumNumberOfCommandLineArgumentsExpected)
				throw new InvalidOperationException(
					$"Invalid number of command line arguments provided. You should provide at least {MinimumNumberOfCommandLineArgumentsExpected} arguments for guishell data. Unable to proceed.");

			var guishellBaseUrl = args[0];
			var applicationName = args[1];
			var guishellSecret = args[2];

			return (guishellBaseUrl, applicationName, guishellSecret);
		}
	}
}
