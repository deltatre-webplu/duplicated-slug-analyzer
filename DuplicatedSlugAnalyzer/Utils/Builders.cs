using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Deltatre.Utils.Extensions.Dictionary;
using DuplicatedSlugAnalyzer.Guishell;
using DuplicatedSlugAnalyzer.Mongodb;

namespace DuplicatedSlugAnalyzer.Utils
{
	public static class Builders
	{
		public static MongodbFactory CreateMongodbFactory(ApplicationConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if(configuration.BackEndStoreConfiguration == null)
				throw new ArgumentNullException(nameof(configuration.BackEndStoreConfiguration));

			if (configuration.DistributionStoreConfiguration == null)
				throw new ArgumentNullException(nameof(configuration.DistributionStoreConfiguration));

			var backendDbConnString = configuration.BackEndStoreConfiguration.ConnectionString;
			var distributionDbConnString = configuration.DistributionStoreConfiguration.ConnectionString;

			return new MongodbFactory(backendDbConnString, distributionDbConnString);
		}

		public static ReadOnlyDictionary<string, string> CreateEntityCodeToDistributionCodeMap(
			ApplicationConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			if (configuration.CustomEntitiesConfiguration == null)
				throw new ArgumentNullException(nameof(configuration.CustomEntitiesConfiguration));

			if (configuration.CustomEntitiesConfiguration.Definitions == null)
				throw new ArgumentNullException(nameof(configuration.CustomEntitiesConfiguration.Definitions));

			return configuration.CustomEntitiesConfiguration.Definitions
				.ToDictionary(
					ce => ce.Code,
					ce => ce.DistributionCode)
				.AsReadOnly();
		}
	}
}
