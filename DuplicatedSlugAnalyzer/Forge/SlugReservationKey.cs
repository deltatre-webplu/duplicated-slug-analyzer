using System;
using System.Collections.Generic;

namespace DuplicatedSlugAnalyzer.Forge
{
	public struct SlugReservationKey : IEquatable<SlugReservationKey>
	{
		public string Slug { get; }
		public string Culture { get; }
		public string EntityType { get; }
		public string EntityCode { get; }

		public SlugReservationKey(string slug, string culture, string entityType, string entityCode)
		{
			Slug = slug;
			Culture = culture;
			EntityType = entityType;
			EntityCode = entityCode;
		}

		public override bool Equals(object obj)
		{
			return obj is SlugReservationKey key && Equals(key);
		}

		public bool Equals(SlugReservationKey other)
		{
			return Slug == other.Slug &&
						 Culture == other.Culture &&
						 EntityType == other.EntityType &&
						 EntityCode == other.EntityCode;
		}

		public override int GetHashCode()
		{
			var hashCode = 1957473872;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Slug);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Culture);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EntityType);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EntityCode);
			return hashCode;
		}

		public static bool operator ==(SlugReservationKey key1, SlugReservationKey key2)
		{
			return key1.Equals(key2);
		}

		public static bool operator !=(SlugReservationKey key1, SlugReservationKey key2)
		{
			return !(key1 == key2);
		}
	}
}
