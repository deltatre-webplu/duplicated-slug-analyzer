using MongoDB.Bson;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Mongodb
{
	public static class DuplicateSlugsFinderFactory
	{
		private const string CollectionName = "wcm.EntitiesPublished";

		public static DuplicateSlugsFinder Create(string connString)
		{
			var mongoUrl = MongoUrl.Create(connString);
			var databaseName = mongoUrl.DatabaseName;

			var mongoClient = new MongoClient(connString);
			var database = mongoClient.GetDatabase(databaseName);
			var collection = database.GetCollection<BsonDocument>(CollectionName);

			return new DuplicateSlugsFinder(collection);
		}
	}
}
