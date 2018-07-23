using System;

namespace DuplicatedSlugAnalyzer.Contracts
{
	public class WcmEntity
	{
		public string EntityType { get; }
		public string EntityCode { get; }
		public string Culture { get; }
		public string Slug { get; }
		public Guid EntityId { get; }
		public Guid TranslationId { get; }

		public WcmEntity(
			string entityType, 
			string entityCode, 
			string culture, 
			string slug,
			Guid entityId, 
			Guid translationId)
		{
			EntityType = entityType;
			EntityCode = entityCode;
			Culture = culture;
			Slug = slug;
			EntityId = entityId;
			TranslationId = translationId;
		}
	}
}
