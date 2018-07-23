using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Mongodb
{
	public static class DuplicateSlugsFinderFactory
	{
		private const string CollectionName = "wcm.EntitiesPublished";

		public static DuplicateSlugsFinder Create(string connString)
		{
			if(string.IsNullOrWhiteSpace(connString))
				throw new ArgumentException("Mongodb connection string cannot be null or white space.", nameof(connString));

			var mongoUrl = MongoUrl.Create(connString);
			var databaseName = mongoUrl.DatabaseName;

			var mongoClient = new MongoClient(connString);
			var database = mongoClient.GetDatabase(databaseName);
			var collection = database.GetCollection<BsonDocument>(CollectionName);

			return new DuplicateSlugsFinder(collection);
		}
	}
}
