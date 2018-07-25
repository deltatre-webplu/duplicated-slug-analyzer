using System;
using Microsoft.Extensions.Configuration;

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

		public static void EnsureValidConfiguration(IConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));
		}
	}
}
