using System;
using DuplicatedSlugAnalyzer.Contracts;

namespace DuplicatedSlugAnalyzer.Distribution
{
	public class DistributionEntityChecker
	{
		private readonly DistributionCollectionFactory _factory;

		public DistributionEntityChecker(DistributionCollectionFactory factory)
		{
			_factory = factory ?? throw new ArgumentNullException(nameof(factory));
		}

		public bool ExistsInDistribution(WcmEntity wcmEntity)
		{
			if (wcmEntity == null)
				throw new ArgumentNullException(nameof(wcmEntity));

			var collection =
				_factory.GetDistributionResourceCollection(
					wcmEntity.Culture,
					wcmEntity.EntityType,
					wcmEntity.EntityCode);
		}
	}
}
