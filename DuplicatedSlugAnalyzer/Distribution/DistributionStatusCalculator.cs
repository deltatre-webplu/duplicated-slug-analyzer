using System;
using System.Threading.Tasks;
using DuplicatedSlugAnalyzer.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Distribution
{
	public class DistributionStatusCalculator
	{
		private readonly DistributionCollectionFactory _factory;

		public DistributionStatusCalculator(DistributionCollectionFactory factory)
		{
			_factory = factory ?? throw new ArgumentNullException(nameof(factory));
		}

		public async Task<DistributionStatus> GetEntityDistributionStatusAsync(WcmEntity wcmEntity)
		{
			if (wcmEntity == null)
				throw new ArgumentNullException(nameof(wcmEntity));

			var collection =
				_factory.GetDistributionResourceCollection(
					wcmEntity.Culture,
					wcmEntity.EntityType,
					wcmEntity.EntityCode);

			if (collection.IsNone)
				return DistributionStatus.Unknown;


			

			throw new NotImplementedException();
		}

		private static async Task<bool> IsTranslationInCollectionAsync(
			Guid translationId, 
			IMongoCollection<BsonDocument> collection)
		{
			collection.Find(d => d["_translationId"] == translationId);
			throw new NotImplementedException();
		}
	}
}
