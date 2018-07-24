using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DuplicatedSlugAnalyzer.Forge;
using Newtonsoft.Json;
using static DuplicatedSlugAnalyzer.Utils.EnvironmentHelpers;
using static System.IO.Path;

namespace DuplicatedSlugAnalyzer.Report
{
	public static class JsonHelpers
	{
		public static string GetReportDirectoryPath(string reportDirectoryName)
		{
			if (string.IsNullOrWhiteSpace(reportDirectoryName))
				throw new ArgumentException($"Parameter '{nameof(reportDirectoryName)}' cannot be null or white space.", nameof(reportDirectoryName));

			return Combine(GetRunningAssemblyDirectoryPath(), reportDirectoryName);
		}

		public static string GetReportFilePath(string reportFileName, string reportDirectoryName)
		{
			if (string.IsNullOrWhiteSpace(reportFileName))
				throw new ArgumentException($"Parameter '{nameof(reportFileName)}' cannot be null or white space.", nameof(reportFileName));

			return Combine(GetReportDirectoryPath(reportDirectoryName), reportFileName);
		}

		public static async Task CreateJsonReportAsync(
			IEnumerable<DuplicateSlugInfo> duplicateSlugsInfos, 
			string reportFileName, 
			string reportDirectoryName)
		{
			if (duplicateSlugsInfos == null)
				throw new ArgumentNullException(nameof(duplicateSlugsInfos));

			var reportFilePath = GetReportFilePath(reportFileName, reportDirectoryName);
			EnsureReportDirectoryExists(reportFilePath);

			var settings = new JsonSerializerSettings()
			{
				Formatting = Formatting.Indented
			};
			var json = JsonConvert.SerializeObject(duplicateSlugsInfos, settings);

			await File.WriteAllTextAsync(reportFilePath, json).ConfigureAwait(false);
		}

		private static void EnsureReportDirectoryExists(string reportFilePath) => 
			CreateDirectoryIfNotExisting(GetDirectoryName(reportFilePath));
	}
}
