using System;
using System.Linq;
using System.Threading.Tasks;
using DuplicatedSlugAnalyzer.Distribution;
using DuplicatedSlugAnalyzer.Forge;

namespace DuplicatedSlugAnalyzer.Report
{
	public static class ReportHelpers
	{
		public static async Task<DuplicateSlugReport> CreateReportAsync(
			DuplicateSlugInfo info,
			PublishedEntityFinder finder)
		{
			if (info == null)
				throw new ArgumentNullException(nameof(info));

			if (finder == null)
				throw new ArgumentNullException(nameof(finder));

			var publishedEntityIdentifier = await finder
				.FindPublishedEntityAsync(info.Key)
				.ConfigureAwait(false);

			return publishedEntityIdentifier.Match(
				identifier =>
					{
						Func<EntityIdentifier, ForgeEntity> projector = x => 
							new ForgeEntity(x.EntityId, x.TranslationId, x == identifier);
						return ToReport(info, projector);
					},
				() =>
					{
						Func<EntityIdentifier, ForgeEntity> projector = x => 
							new ForgeEntity(x.EntityId, x.TranslationId, false);
						return ToReport(info, projector);
					}
			);
		}

		private static DuplicateSlugReport ToReport(
			DuplicateSlugInfo info, 
			Func<EntityIdentifier, ForgeEntity> projector)
		{
			var forgeEntities = info.EntityIdentifiers.Select(projector).ToHashSet();
			return new DuplicateSlugReport(info.Key, forgeEntities);
		}
	}
}
