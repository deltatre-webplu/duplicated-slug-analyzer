﻿namespace DuplicatedSlugAnalyzer
{
	public static class Constants
	{
		public const string ConfigurationFileName = "appsettings.json";
		public const string ReportFileName = "duplicate-slug-analyzer-report.json";
		public const string DefaultReportDirectoryName = "Reports";
		public const string DefaultLogsDirectoryName = "Logs";
		public const string GuishellBaseUrlConfigKey = "guishellBaseUrl";
		public const string ApplicationNameConfigKey = "applicationName";
		public const string GuishellSecretConfigKey = "guishellSecret";
		public const int BatchSize = 20;
		public const string ReportDirectoryPathConfigKey = "reportDirectoryPath";
		public const string LogsDirectoryPathConfigKey = "logsDirectoryPath";
		public const string LogFileName = "duplicate-slug-analyzer-logs.txt";
	}
}
