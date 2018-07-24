using System;
using System.Collections.Generic;

namespace DuplicatedSlugAnalyzer.Forge
{
	public struct EntityIdentifier : IEquatable<EntityIdentifier>
	{
		public Guid EntityId { get; }
		public Guid TranslationId { get; }

		public EntityIdentifier(Guid entityId, Guid translationId)
		{
			EntityId = entityId;
			TranslationId = translationId;
		}

		public override bool Equals(object obj)
		{
			return obj is EntityIdentifier identifier && Equals(identifier);
		}

		public bool Equals(EntityIdentifier other)
		{
			return EntityId.Equals(other.EntityId) &&
						 TranslationId.Equals(other.TranslationId);
		}

		public override int GetHashCode()
		{
			var hashCode = -1448544816;
			hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(EntityId);
			hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(TranslationId);
			return hashCode;
		}

		public static bool operator ==(EntityIdentifier identifier1, EntityIdentifier identifier2)
		{
			return identifier1.Equals(identifier2);
		}

		public static bool operator !=(EntityIdentifier identifier1, EntityIdentifier identifier2)
		{
			return !(identifier1 == identifier2);
		}
	}
}
