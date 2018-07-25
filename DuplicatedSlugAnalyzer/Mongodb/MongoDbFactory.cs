using System;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Mongodb
{
	public class MongodbFactory
	{
		private readonly Lazy<IMongoDatabase> _backendDatabase;
		private readonly Lazy<IMongoDatabase> _distributionDatabase;

		public MongodbFactory(
			string backendDbConnString, 
			string distributionDbConnString)
		{
			if(string.IsNullOrWhiteSpace(backendDbConnString))
				throw new ArgumentException("Backend store connection string cannot be null or white space.", nameof(backendDbConnString));

			if(string.IsNullOrWhiteSpace(distributionDbConnString))
				throw new ArgumentException("Distribution store connection string cannot be null or white space.", nameof(distributionDbConnString));

			_backendDatabase = new Lazy<IMongoDatabase>(() => GetDatabaseFromConnString(backendDbConnString));
			_distributionDatabase = new Lazy<IMongoDatabase>(() => GetDatabaseFromConnString(distributionDbConnString));
		}

		public IMongoDatabase DistributionDatabase => _distributionDatabase.Value;
		public IMongoDatabase BackendDatabase => _backendDatabase.Value;

		private static IMongoDatabase GetDatabaseFromConnString(string connString)
		{
			var mongoUrl = MongoUrl.Create(connString);
			var databaseName = mongoUrl.DatabaseName;

			var mongoClient = new MongoClient(connString);
			var database = mongoClient.GetDatabase(databaseName);
			return database;
		}
	}
}
