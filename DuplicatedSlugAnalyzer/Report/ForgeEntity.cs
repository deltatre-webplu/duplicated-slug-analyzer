using System;
using Newtonsoft.Json;

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
		[JsonProperty(PropertyName = "IsInDistribution")]
		public bool IsPublished { get; }
	}
}
