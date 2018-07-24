using System;
using System.Collections.Concurrent;
using LanguageExt;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Distribution
{
	public class CachedDistributionCollectionFactory: IDistributionCollectionFactory
	{
		private readonly IDistributionCollectionFactory _factory;
		private readonly ConcurrentDictionary<DistributionResource, Option<IMongoCollection<BsonDocument>>> _cache;

		public CachedDistributionCollectionFactory(IDistributionCollectionFactory factory)
		{
			_factory = factory ?? throw new ArgumentNullException(nameof(factory));
			_cache = new ConcurrentDictionary<DistributionResource, Option<IMongoCollection<BsonDocument>>>();
		}

		public Option<IMongoCollection<BsonDocument>> GetDistributionResourceCollection(
			DistributionResource resource)
		{
			var mongoCollection = _cache.GetOrAdd(
				resource, 
				r => _factory.GetDistributionResourceCollection(r));

			return mongoCollection;
		}
	}
}
