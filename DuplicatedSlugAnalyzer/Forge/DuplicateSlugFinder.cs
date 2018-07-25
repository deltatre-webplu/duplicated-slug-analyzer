using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuplicatedSlugAnalyzer.Forge
{
	public class DuplicateSlugFinder
	{
		private readonly IEnumerable<BaseDuplicateSlugFinder> _finders;

		public DuplicateSlugFinder(IEnumerable<BaseDuplicateSlugFinder> finders)
		{
			_finders = finders ?? throw new ArgumentNullException(nameof(finders));
		}

		public async Task<IEnumerable<DuplicateSlugInfo>> GetDuplicateSlugsInfoAsync()
		{
			var cache = new Dictionary<SlugReservationKey, HashSet<EntityIdentifier>>();

			foreach (var finder in _finders)
			{
				var duplicates = await finder.GetDuplicateSlugsInfoAsync().ConfigureAwait(false);

				foreach (var duplicate in duplicates)
				{
					if (cache.ContainsKey(duplicate.Key))
					{
						cache[duplicate.Key].UnionWith(duplicate.EntityIdentifiers);
					}
					else
					{
						cache.Add(duplicate.Key, duplicate.EntityIdentifiers);
					}
				}
			}

			return cache.Select(kvp => new DuplicateSlugInfo(kvp.Key, kvp.Value));
		}
	}
}

