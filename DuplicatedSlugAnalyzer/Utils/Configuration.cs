﻿using System;
using Microsoft.Extensions.Configuration;
using Serilog;
using static DuplicatedSlugAnalyzer.Constants;

namespace DuplicatedSlugAnalyzer.Utils
{
	public static class Configuration
	{
		public static string ReadSettingFromConfiguration(
			IConfiguration configuration, 
			string key,
			Func<string> defaultValueFactory)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if (string.IsNullOrWhiteSpace(key))
				throw new ArgumentException("Configuration key cannot be null or white space.", nameof(key));

			if (defaultValueFactory == null)
				throw new ArgumentNullException(nameof(defaultValueFactory));

			var configuredValue = configuration[key];

			return string.IsNullOrWhiteSpace(configuredValue)
				? defaultValueFactory()
				: configuredValue;
		}

		public static bool IsValidConfiguration(IConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			return
				HasConfiguration(configuration, GuishellBaseUrlConfigKey,
					() => LogErrorForMissingConfiguration(GuishellBaseUrlConfigKey))
				&&
				HasConfiguration(configuration, ApplicationNameConfigKey,
					() => LogErrorForMissingConfiguration(ApplicationNameConfigKey))
				&&
				HasConfiguration(configuration, GuishellSecretConfigKey,
					() => LogErrorForMissingConfiguration(GuishellSecretConfigKey));
		}

		private static bool HasConfiguration(IConfiguration configuration, string key, Action missingConfigurationCallback)
		{
			var hasConfiguration = !string.IsNullOrWhiteSpace(configuration[key]);
			if (!hasConfiguration)
				missingConfigurationCallback();

			return hasConfiguration;
		}

		private static void LogErrorForMissingConfiguration(string configKey) => 
			Log.Error("Configuration {ConfigKey} is mandatory. Cannot proceed.", configKey);
	}
}
