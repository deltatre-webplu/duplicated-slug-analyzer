using MongoDB.Bson;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Forge
{
	public class EntityPublishedDuplicateSlugsFinder: BaseDuplicateSlugFinder
	{
		public EntityPublishedDuplicateSlugsFinder(IMongoCollection<BsonDocument> collection) : base(collection)
		{
		}

		protected override BsonDocument BuildGroupIdDocument()
		{
			var document = new BsonDocument
			{
				["culture"] = "$TranslationInfo.Culture",
				["entityType"] = "$EntityType",
				["entityCode"] = "$FriendlyName",
				["slug"] = "$Slug"
			};
			return document;
		}

		public override string FinderName => "EntityPublished";
	}
}
