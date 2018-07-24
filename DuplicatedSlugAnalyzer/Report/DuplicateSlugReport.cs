using System.Collections.Generic;
using DuplicatedSlugAnalyzer.Forge;

namespace DuplicatedSlugAnalyzer.Report
{
	public class DuplicateSlugReport
	{
		public SlugReservationKey Key { get; }
		public int NumberOfEntities { get; }
		public IEnumerable<ForgeEntity> Entities { get; }

		public DuplicateSlugReport(
			SlugReservationKey key,
			int numberOfEntities,
			IEnumerable<ForgeEntity> entities)
		{
			Key = key;
			NumberOfEntities = numberOfEntities;
			Entities = entities;
		}
	}
}
