using System;

namespace DuplicatedSlugAnalyzer.Forge
{
	public struct EntityIdentifier
	{
		public Guid EntityId { get; }
		public Guid TranslationId { get; }

		public EntityIdentifier(Guid entityId, Guid translationId)
		{
			EntityId = entityId;
			TranslationId = translationId;
		}
	}
}
