using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Forge
{
	public class BuiltInEntityDuplicateSlugFinder: BaseDuplicateSlugFinder
	{
		private readonly string _entityType;

		public BuiltInEntityDuplicateSlugFinder(
			IMongoCollection<BsonDocument> collection, 
			string entityType) : base(collection)
		{
			if(string.IsNullOrWhiteSpace(entityType))
				throw new ArgumentException("Entity type cannot be null or white space.", nameof(entityType));

			_entityType = entityType;
		}

		protected override BsonDocument BuildGroupIdDocument()
		{
			var document = new BsonDocument
			{
				["culture"] = "$TranslationInfo.Culture",
				["entityType"] = _entityType,
				["entityCode"] = _entityType,
				["slug"] = "$Slug"
			};
			return document;
		}

		public override string FinderName => _entityType;
	}
}
