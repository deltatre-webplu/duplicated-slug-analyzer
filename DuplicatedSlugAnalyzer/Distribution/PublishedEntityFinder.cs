using System;
using System.Threading.Tasks;
using DuplicatedSlugAnalyzer.Mongodb;
using LanguageExt;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Distribution
{
	public class PublishedEntityFinder
	{
		private readonly DistributionCollectionFactory _collectionFactory;

		public PublishedEntityFinder(DistributionCollectionFactory factory)
		{
			_collectionFactory = factory ?? throw new ArgumentNullException(nameof(factory));
		}

		public async Task<Option<EntityIdentifier>> FindPublishedEntityAsync(SlugReservationKey key)
		{
			var collection = _collectionFactory.GetDistributionResourceCollection(
				key.Culture, 
				key.EntityType,
				key.EntityCode);

			var publishedDocument = await collection.MatchAsync(
				async c => await GetPublishedDocumentByIdAsync(c, key.Slug).ConfigureAwait(false),
				() => Option<BsonDocument>.None).ConfigureAwait(false);

			return publishedDocument.Match(
				d => Option<EntityIdentifier>.Some(ToEntityIdentifier(d)),
				() => Option<EntityIdentifier>.None);
		}

		private static async Task<Option<BsonDocument>> GetPublishedDocumentByIdAsync(
			IMongoCollection<BsonDocument> collection, 
			string id)
		{
			var document = await collection
				.Find(d => d["_id"] == id)
				.FirstOrDefaultAsync()
				.ConfigureAwait(false);

			return document != null ? 
				Option<BsonDocument>.Some(document) : 
				Option<BsonDocument>.None;
		}

		private static EntityIdentifier ToEntityIdentifier(BsonValue document)
		{
			var entityId = document["_entityId"].AsGuid;
			var translationId = document["_translationId"].AsGuid;
			return new EntityIdentifier(entityId, translationId);
		}
	}
}
