using System;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Formatting.Json;
using static DuplicatedSlugAnalyzer.Constants;
using static System.IO.Path;
using static DuplicatedSlugAnalyzer.Utils.EnvironmentHelpers;

namespace DuplicatedSlugAnalyzer.Logging
{
	public static class Serilog
	{
		public static ILogger CreateLogger(string logsDirectoryPath)
		{
			if(string.IsNullOrWhiteSpace(logsDirectoryPath))
				throw new ArgumentException("Logs directory path cannot be null or white space.", nameof(logsDirectoryPath));

			var logFilePath = Combine(logsDirectoryPath, LogFileName);

			var logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.File(new JsonFormatter(), logFilePath, rollingInterval: RollingInterval.Day)
				.WriteTo.Console(LogEventLevel.Information)
				.CreateLogger();

			return logger;
		}

		public static string GetDefaultLogsDirectoryPath()
		{
			var runningAssemblyDirectoryPath = GetRunningAssemblyDirectoryPath();
			return Combine(runningAssemblyDirectoryPath, DefaultLogsDirectoryName);
		}
	}
}
