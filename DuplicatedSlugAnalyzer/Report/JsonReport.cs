﻿using System;
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
		public static string BuildDefaultReportDirectoryPath(string reportDirectoryName)
		{
			if (string.IsNullOrWhiteSpace(reportDirectoryName))
				throw new ArgumentException($"Parameter '{nameof(reportDirectoryName)}' cannot be null or white space.", nameof(reportDirectoryName));

			return Combine(GetRunningAssemblyDirectoryPath(), reportDirectoryName);
		}

		public static string GetReportFilePath(string reportFileName, string reportDirectoryName)
		{
			if (string.IsNullOrWhiteSpace(reportFileName))
				throw new ArgumentException($"Parameter '{nameof(reportFileName)}' cannot be null or white space.", nameof(reportFileName));

			return Combine(BuildDefaultReportDirectoryPath(reportDirectoryName), reportFileName);
		}

		public static async Task CreateJsonReportAsync(
			IEnumerable<DuplicateSlugReport> duplicateSlugsReports, 
			string reportFileName, 
			string reportDirectoryName)
		{
			if (duplicateSlugsReports == null)
				throw new ArgumentNullException(nameof(duplicateSlugsReports));

			var reportFilePath = GetReportFilePath(reportFileName, reportDirectoryName);
			EnsureReportDirectoryExists(reportFilePath);

			var settings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented
			};
			var json = JsonConvert.SerializeObject(duplicateSlugsReports, settings);

			await File.WriteAllTextAsync(reportFilePath, json).ConfigureAwait(false);
		}

		private static void EnsureReportDirectoryExists(string reportFilePath) => 
			CreateDirectoryIfNotExisting(GetDirectoryName(reportFilePath));

		public static string GetDefaultReportReportFilePath()
		{
			var directoryPath = BuildDefaultReportDirectoryPath(ReportDirectoryName);
			return Combine(directoryPath, ReportFileName);
		}
	}
}