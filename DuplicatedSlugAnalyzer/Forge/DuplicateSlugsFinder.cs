using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Forge
{
	public class DuplicateSlugsFinder
	{
		private readonly IMongoCollection<BsonDocument> _publishedEntitiesCollection;

		public DuplicateSlugsFinder(IMongoCollection<BsonDocument> publishedEntitiesCollection)
		{
			_publishedEntitiesCollection = publishedEntitiesCollection;
		}

		public async Task<IEnumerable<DuplicateSlugInfo>> GetDuplicateSlugsInfoAsync()
		{
			var options = new AggregateOptions
			{
				AllowDiskUse = true
			};
			var groupDocument = BuildGroupDocument();
			var matchDocument = BuildMatchDocument();

			var documents = await _publishedEntitiesCollection
				.Aggregate(options)
				.Group(groupDocument)
				.Match(matchDocument)
				.ToListAsync()
				.ConfigureAwait(false);

			var result = documents.Select(ToSlugReservationKeyInfo).ToArray();
			return result;
		}

		private static DuplicateSlugInfo ToSlugReservationKeyInfo(BsonDocument document)
		{
			var numberOfEntities = document["count"].AsInt32;

			var culture = document["_id"]["culture"].AsString;
			var entityType = document["_id"]["entityType"].AsString;
			var entityCode = document["_id"]["entityCode"].IsBsonNull ? null : document["_id"]["entityCode"].AsString;
			var slug = document["_id"]["slug"].IsBsonNull ? null : document["_id"]["slug"].AsString;

			var key = new SlugReservationKey(slug, culture, entityType, entityCode);

			var identifiers = document["identifiers"].AsBsonArray.Select(d => ToEntityIdentifier(d.AsBsonDocument)).ToArray();

			return new DuplicateSlugInfo(key, numberOfEntities, identifiers);
		}

		private static EntityIdentifier ToEntityIdentifier(BsonDocument document)
		{
			var entityId = document["entityId"].AsGuid;
			var translationId = document["translationId"].AsGuid;
			return new EntityIdentifier(entityId, translationId);
		}

		private static BsonDocument BuildGroupDocument()
		{
			var idDocument = new BsonDocument
			{
				["culture"] = "$TranslationInfo.Culture",
				["entityType"] = "$EntityType",
				["entityCode"] = "$FriendlyName",
				["slug"] = "$Slug"
			};

			var countDocument = new BsonDocument("$sum", 1);

			var entityIdentifierDocument = new BsonDocument
			{
				["entityId"] = "$EntityId",
				["translationId"] = "$_id"
			};
			var identifierDocument = new BsonDocument
			{
				["$addToSet"] = entityIdentifierDocument
			};
			
			var groupDocument = new BsonDocument
			{
				["_id"] = idDocument,
				["count"] = countDocument,
				["identifiers"] = identifierDocument
			};
			return groupDocument;
		}

		private static BsonDocument BuildMatchDocument()
		{
			var countDocument = new BsonDocument
			{
				["$gt"] = 1
			};

			var matchDocument = new BsonDocument
			{
				["count"] = countDocument
			};
			return matchDocument;
		}
	}
}
