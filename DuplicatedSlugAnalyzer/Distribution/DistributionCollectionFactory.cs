using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using static DuplicatedSlugAnalyzer.Mongodb.MongoDbHelpers;

namespace DuplicatedSlugAnalyzer.Distribution
{
	public class DistributionCollectionFactory
	{
		private readonly IMongoDatabase _database;
		private readonly IReadOnlyDictionary<string, string> _entityCodeToDistributionCode;
		private const string CollectionNameSeparator = "__";

		public static DistributionCollectionFactory Create(
			string connString, 
			IReadOnlyDictionary<string, string> entityCodeToDistributionCode)
		{
			if (entityCodeToDistributionCode == null)
				throw new ArgumentNullException(nameof(entityCodeToDistributionCode));

			var database = GetDatabaseFromConnString(connString);

			return new DistributionCollectionFactory(database, entityCodeToDistributionCode);
		}

		private DistributionCollectionFactory(
			IMongoDatabase database, 
			IReadOnlyDictionary<string, string> entityCodeToDistributionCode)
		{
			_database = database;
			_entityCodeToDistributionCode = entityCodeToDistributionCode;
		}

		public IMongoCollection<BsonDocument> GetCollection(
			string culture, 
			string entityType, 
			string entityCode)
		{
			throw new NotImplementedException();
		}

		private string BuildCollectionName(
			string culture,
			string entityType,
			string entityCode)
		{
			var resourceName = GetDistributionResourceName(entityType, entityCode);
			return $"{culture}{CollectionNameSeparator}{resourceName}";
		}

		private string GetDistributionResourceName(
			string entityType,
			string entityCode)
		{
			switch (entityType.ToLowerInvariant())
			{
				case "album":
					return "albums";

				case "photo":
					return "photos";

				case "document":
					return "documents";

				case "story":
					return "stories";

				case "tag":
					return "tags";

				case "customentity":
					return LookupDistributionCode(entityCode);

				case "selection":
					throw new NotSupportedException($"Entity type not directly mapped to a distribution resource: '{entityType}'.");

				default:
					throw new UnknownEntityTypeException($"Unknown entity type: '{entityType}'.");
			}
		}

		private string LookupDistributionCode(string entityCode)
		{
			var distributionCodeFound = _entityCodeToDistributionCode.TryGetValue(
				entityCode, 
				out var distributionCode);
			if(!distributionCodeFound)
				throw new DistributionCodeNotFoundException($"Unable to find distribution code corresponding to entity code '{entityCode}'.");

			return distributionCode;
		}
	}
}
