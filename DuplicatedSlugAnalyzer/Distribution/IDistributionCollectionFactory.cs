using LanguageExt;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Distribution
{
	public interface IDistributionCollectionFactory
	{
		Option<IMongoCollection<BsonDocument>> GetDistributionResourceCollection(DistributionResource resource);
	}
}