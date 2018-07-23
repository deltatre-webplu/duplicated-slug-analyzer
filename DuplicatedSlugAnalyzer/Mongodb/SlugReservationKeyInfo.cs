using System.Collections.Generic;

namespace DuplicatedSlugAnalyzer.Mongodb
{
	public class SlugReservationKeyInfo
	{
		public SlugReservationKey Key { get; }
		public int NumberOfEntities { get; }
		public IEnumerable<EntityIdentifier> EntityIdentifiers { get; }

		public SlugReservationKeyInfo(
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
