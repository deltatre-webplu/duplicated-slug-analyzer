using System;
using System.Collections.Generic;

namespace DuplicatedSlugAnalyzer.Distribution
{
	public struct DistributionResource : IEquatable<DistributionResource>
	{
		public DistributionResource(string entityType, string entityCode, string culture)
		{
			EntityType = entityType;
			EntityCode = entityCode;
			Culture = culture;
		}

		public string EntityType { get; }
		public string EntityCode { get; }
		public string Culture { get; }

		public override bool Equals(object obj)
		{
			return obj is DistributionResource resource && Equals(resource);
		}

		public bool Equals(DistributionResource other)
		{
			return EntityType == other.EntityType &&
						 EntityCode == other.EntityCode &&
						 Culture == other.Culture;
		}

		public override int GetHashCode()
		{
			var hashCode = -71457446;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EntityType);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EntityCode);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Culture);
			return hashCode;
		}

		public static bool operator ==(DistributionResource resource1, DistributionResource resource2)
		{
			return resource1.Equals(resource2);
		}

		public static bool operator !=(DistributionResource resource1, DistributionResource resource2)
		{
			return !(resource1 == resource2);
		}
	}
}
