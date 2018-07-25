using System;

namespace DuplicatedSlugAnalyzer.Report
{
	public class ForgeEntity
	{
		public ForgeEntity(Guid entityId, Guid translationId, bool isPublished)
		{
			EntityId = entityId;
			TranslationId = translationId;
			IsPublished = isPublished;
		}

		public Guid EntityId { get; }
		public Guid TranslationId { get; }
		public bool IsPublished { get; }
	}
}
