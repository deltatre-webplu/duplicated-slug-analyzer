using System;
using System.Collections.ObjectModel;
using System.Linq;
using Deltatre.Utils.Extensions.Dictionary;
using DuplicatedSlugAnalyzer.Distribution;
using DuplicatedSlugAnalyzer.Forge;
using DuplicatedSlugAnalyzer.Guishell;
using DuplicatedSlugAnalyzer.Mongodb;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;

namespace DuplicatedSlugAnalyzer.Utils
{
	public static class Builders
	{
		private const string EntitiesPublishedCollectionName = "wcm.EntitiesPublished";
		private const string AlbumsPublishedCollectionName = "wcm.AlbumsPublished";
		private const string CustomEntitiesPublishedCollectionName = "wcm.CustomEntitiesPublished";
		private const string DocumentsPublishedCollectionName = "wcm.DocumentsPublished";
		private const string PhotosPublishedCollectionName = "wcm.PhotosPublished";
		private const string SelectionsPublishedCollectionName = "wcm.SelectionsPublished";
		private const string StoriesPublishedCollectionName = "wcm.StoriesPublished";
		private const string TagsPublishedCollectionName = "wcm.TagsPublished";

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

			Log.Debug("Backend database connection string is {BackendDbConnString}", backendDbConnString);
			Log.Debug("Distribution database connection string is {DistributionDbConnString}", distributionDbConnString);

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

		public static DuplicateSlugFinder CreateDuplicateSlugFinder(IMongoDatabase backofficeDatabase)
		{
			var entitiesPublishedColl = backofficeDatabase.GetCollection<BsonDocument>(EntitiesPublishedCollectionName);
			var albumsPublishedColl = backofficeDatabase.GetCollection<BsonDocument>(AlbumsPublishedCollectionName);
			var customEntitiesPublishedColl = backofficeDatabase.GetCollection<BsonDocument>(CustomEntitiesPublishedCollectionName);
			var documentsPublishedColl = backofficeDatabase.GetCollection<BsonDocument>(DocumentsPublishedCollectionName);
			var photosPublishedColl = backofficeDatabase.GetCollection<BsonDocument>(PhotosPublishedCollectionName);
			var selectionsPublishedColl = backofficeDatabase.GetCollection<BsonDocument>(SelectionsPublishedCollectionName);
			var storiesPublishedColl = backofficeDatabase.GetCollection<BsonDocument>(StoriesPublishedCollectionName);
			var tagsPublishedColl = backofficeDatabase.GetCollection<BsonDocument>(TagsPublishedCollectionName);

			var finders = new BaseDuplicateSlugFinder[]
			{
				new EntityPublishedDuplicateSlugsFinder(entitiesPublishedColl),
				new BuiltInEntityDuplicateSlugFinder(albumsPublishedColl, "album"),
				new BuiltInEntityDuplicateSlugFinder(documentsPublishedColl, "document"),
				new BuiltInEntityDuplicateSlugFinder(photosPublishedColl, "photo"),
				new BuiltInEntityDuplicateSlugFinder(selectionsPublishedColl, "selection"),
				new BuiltInEntityDuplicateSlugFinder(storiesPublishedColl, "story"),
				new BuiltInEntityDuplicateSlugFinder(tagsPublishedColl, "tag"),
				new CustomEntityDuplicateSlugFinder(customEntitiesPublishedColl) 
			};

			return new DuplicateSlugFinder(finders);
		}
	}
}
