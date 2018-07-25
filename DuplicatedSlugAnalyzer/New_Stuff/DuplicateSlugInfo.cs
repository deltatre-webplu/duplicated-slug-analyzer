using System.Collections.Generic;
using DuplicatedSlugAnalyzer.Forge;

namespace DuplicatedSlugAnalyzer.New_Stuff
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
