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

		public IMongoCollection<BsonDocument> GetCollection()
		{
			throw new NotImplementedException();
		}
	}
}
