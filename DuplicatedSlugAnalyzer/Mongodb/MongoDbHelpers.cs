using System;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Mongodb
{
	public static class MongoDbHelpers
	{
		public static IMongoDatabase GetDatabaseFromConnString(string connString)
		{
			if (string.IsNullOrWhiteSpace(connString))
				throw new ArgumentException("Mongodb connection string cannot be null or white space.", nameof(connString));

			var mongoUrl = MongoUrl.Create(connString);
			var databaseName = mongoUrl.DatabaseName;

			var mongoClient = new MongoClient(connString);
			var database = mongoClient.GetDatabase(databaseName);

			return database;
		}
	}
}
