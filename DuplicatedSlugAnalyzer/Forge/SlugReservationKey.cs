namespace DuplicatedSlugAnalyzer.Forge
{
	public struct SlugReservationKey
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
	}
}
