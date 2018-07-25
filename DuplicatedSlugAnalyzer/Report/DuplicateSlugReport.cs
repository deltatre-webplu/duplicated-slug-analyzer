using System.Collections.Generic;
using DuplicatedSlugAnalyzer.Forge;
using Newtonsoft.Json;

namespace DuplicatedSlugAnalyzer.Report
{
	public class DuplicateSlugReport
	{
		[JsonProperty(PropertyName = "SlugReservationKey")]
		public SlugReservationKey Key { get; }

		public HashSet<ForgeEntity> Entities { get; }

		public int NumberOfEntities => Entities.Count;

		public DuplicateSlugReport(
			SlugReservationKey key,
			HashSet<ForgeEntity> entities)
		{
			Key = key;
			Entities = entities;
		}
	}
}
