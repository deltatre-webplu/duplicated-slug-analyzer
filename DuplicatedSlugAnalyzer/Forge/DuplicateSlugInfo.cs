using System.Collections.Generic;

namespace DuplicatedSlugAnalyzer.Forge
{
	public class DuplicateSlugInfo
	{
		public SlugReservationKey Key { get; }
		public int NumberOfEntities { get; }
		public IEnumerable<EntityIdentifier> EntityIdentifiers { get; }

		public DuplicateSlugInfo(
			SlugReservationKey key, 
			int numberOfEntities, 
			IEnumerable<EntityIdentifier> entityIdentifiers)
		{
			Key = key;
			NumberOfEntities = numberOfEntities;
			EntityIdentifiers = entityIdentifiers;
		}
	}
}
