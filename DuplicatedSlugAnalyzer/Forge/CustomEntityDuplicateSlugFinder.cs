using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Forge
{
	public class CustomEntityDuplicateSlugFinder
	{
		private readonly IMongoCollection<BsonDocument> _collection;

		public CustomEntityDuplicateSlugFinder(IMongoCollection<BsonDocument> collection)
		{
			_collection = collection ?? throw new ArgumentNullException(nameof(collection));
		}

		public async Task<IEnumerable<DuplicateSlugInfo>> GetDuplicateSlugsInfoAsync()
		{
			var options = new AggregateOptions
			{
				AllowDiskUse = true
			};
			var groupDocument = BuildGroupDocument();
			var matchDocument = BuildMatchDocument();
			var projectDocument = BuildProjectDocument();

			var documents = await _collection
				.Aggregate(options)
				.Group(groupDocument)
				.Match(matchDocument)
				.Project(projectDocument)
				.ToListAsync()
				.ConfigureAwait(false);

			var result = documents.Select(ToSlugReservationKeyInfo).ToArray();
			return result;
		}

		private static DuplicateSlugInfo ToSlugReservationKeyInfo(BsonDocument document)
		{
			var culture = document["_id"]["culture"].AsString;
			var entityType = document["_id"]["entityType"].AsString;
			var entityCode = document["_id"]["entityCode"].IsBsonNull ? null : document["_id"]["entityCode"].AsString;
			var slug = document["_id"]["slug"].IsBsonNull ? null : document["_id"]["slug"].AsString;

			var key = new SlugReservationKey(
				slug,
				culture,
				entityType,
				entityCode);

			var identifiers = document["identifiers"]
				.AsBsonArray
				.Select(d => ToEntityIdentifier(d.AsBsonDocument))
				.ToHashSet();

			return new DuplicateSlugInfo(key, identifiers);
		}

		private static EntityIdentifier ToEntityIdentifier(BsonValue document)
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
				["entityType"] = "customentity",
				["entityCode"] = "$EntityCode",
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

		private static BsonDocument BuildProjectDocument()
		{
			var document = new BsonDocument
			{
				["identifiers"] = 1
			};
			return document;
		}
	}
}
