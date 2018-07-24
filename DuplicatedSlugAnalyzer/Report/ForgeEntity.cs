using DuplicatedSlugAnalyzer.Forge;

namespace DuplicatedSlugAnalyzer.Report
{
	public class ForgeEntity
	{
		public ForgeEntity(EntityIdentifier identifier, bool isPublished)
		{
			Identifier = identifier;
			IsPublished = isPublished;
		}

		public EntityIdentifier Identifier { get; }
		public bool IsPublished { get; }
	}
}
