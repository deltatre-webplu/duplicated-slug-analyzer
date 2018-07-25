using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static DuplicatedSlugAnalyzer.Utils.EnvironmentHelpers;
using static System.IO.Path;
using static DuplicatedSlugAnalyzer.Constants;

namespace DuplicatedSlugAnalyzer.Report
{
	public static class JsonReport
	{
		public static async Task CreateJsonReportAsync(
			IEnumerable<DuplicateSlugReport> duplicateSlugsReports,
			string reportDirectoryPath)
		{
			if (duplicateSlugsReports == null)
				throw new ArgumentNullException(nameof(duplicateSlugsReports));

			if (string.IsNullOrWhiteSpace(reportDirectoryPath))
				throw new ArgumentException("Report directory path cannot be null or white space.", nameof(reportDirectoryPath));

			CreateDirectoryIfNotExisting(reportDirectoryPath);

			var settings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented
			};
			var json = JsonConvert.SerializeObject(duplicateSlugsReports, settings);

			var reportFilePath = Combine(reportDirectoryPath, ReportFileName);
			await File.WriteAllTextAsync(reportFilePath, json).ConfigureAwait(false);
		}

		public static string GetDefaultReportDirectoryPath()
		{
			var runningAssemblyDirectoryPath = GetRunningAssemblyDirectoryPath();
			return Combine(runningAssemblyDirectoryPath, DefaultReportDirectoryName);
		}
	}
}
