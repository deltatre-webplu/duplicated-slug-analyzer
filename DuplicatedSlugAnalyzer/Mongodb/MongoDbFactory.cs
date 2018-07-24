using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Mongodb
{
	public class MongoDbFactory
	{
		private const string PublishedEntitiesCollectionName = "wcm.EntitiesPublished";

		private readonly Lazy<IMongoCollection<BsonDocument>> _publishedEntitisCollection;
		private readonly Lazy<IMongoDatabase> _distributionDatabase;

		public MongoDbFactory(
			string backendDbConnString, 
			string distributionDbConnString)
		{
			if(string.IsNullOrWhiteSpace(backendDbConnString))
				throw new ArgumentException("Backend store connection string cannot be null or white space.", nameof(backendDbConnString));

			if(string.IsNullOrWhiteSpace(distributionDbConnString))
				throw new ArgumentException("Distribution store connection string cannot be null or white space.", nameof(distributionDbConnString));

			_publishedEntitisCollection = 
				new Lazy<IMongoCollection<BsonDocument>>(() => InitPublishedEntitiesCollection(backendDbConnString));

			_distributionDatabase = new Lazy<IMongoDatabase>(() => GetDatabaseFromConnString(distributionDbConnString));
		}

		public IMongoDatabase DistributionDatabase => _distributionDatabase.Value;
		public IMongoCollection<BsonDocument> PublishedEntitiesCollection => _publishedEntitisCollection.Value;

		private static IMongoCollection<BsonDocument> InitPublishedEntitiesCollection(string backendDbConnString)
		{
			var backendDatabase = GetDatabaseFromConnString(backendDbConnString);

			return backendDatabase.GetCollection<BsonDocument>(
				PublishedEntitiesCollectionName,
				new MongoCollectionSettings
				{
					GuidRepresentation = GuidRepresentation.CSharpLegacy
				});
		}

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
