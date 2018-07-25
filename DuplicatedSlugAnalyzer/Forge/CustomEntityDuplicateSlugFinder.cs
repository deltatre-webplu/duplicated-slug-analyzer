using MongoDB.Bson;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Forge
{
	public class CustomEntityDuplicateSlugFinder: BaseDuplicateSlugFinder
	{
		public CustomEntityDuplicateSlugFinder(IMongoCollection<BsonDocument> collection) : base(collection)
		{
		}

		protected override BsonDocument BuildGroupIdDocument()
		{
			var document = new BsonDocument
			{
				["culture"] = "$TranslationInfo.Culture",
				["entityType"] = "customentity",
				["entityCode"] = "$EntityCode",
				["slug"] = "$Slug"
			};
			return document;
		}

		public override string FinderName => "customentity";
	}
}
