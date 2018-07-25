using System.Collections.Generic;

namespace DuplicatedSlugAnalyzer.Forge
{
	public class DuplicateSlugInfo
	{
		public SlugReservationKey Key { get; }
		public HashSet<EntityIdentifier> EntityIdentifiers { get; }

		public DuplicateSlugInfo(
			SlugReservationKey key, 
			HashSet<EntityIdentifier> entityIdentifiers)
		{
			Key = key;
			EntityIdentifiers = entityIdentifiers;
		}
	}
}
