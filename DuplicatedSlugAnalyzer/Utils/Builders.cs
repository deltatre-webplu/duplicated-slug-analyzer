using System;
using System.Collections.ObjectModel;
using System.Linq;
using Deltatre.Utils.Extensions.Dictionary;
using DuplicatedSlugAnalyzer.Distribution;
using DuplicatedSlugAnalyzer.Guishell;
using DuplicatedSlugAnalyzer.Mongodb;
using MongoDB.Driver;

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

		public static PublishedEntityFinder CreatePublishedEntityFinder(
			IMongoDatabase distributionDatabase, 
			ApplicationConfiguration configuration)
		{
			if (distributionDatabase == null)
				throw new ArgumentNullException(nameof(distributionDatabase));

			var entityCodeToDistributionCodeMap = CreateEntityCodeToDistributionCodeMap(configuration);

			var factory = new DistributionCollectionFactory(distributionDatabase, entityCodeToDistributionCodeMap);
			var cachedFactory = new CachedDistributionCollectionFactory(factory);
			var publishedEntityFinder = new PublishedEntityFinder(cachedFactory);

			return publishedEntityFinder;
		}
	}
}
