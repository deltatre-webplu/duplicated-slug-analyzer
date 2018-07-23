using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace DuplicatedSlugAnalyzer.Distribution
{
	public class DistributionCollectionFactory
	{
		private readonly IMongoDatabase _database;

		private DistributionCollectionFactory(IMongoDatabase database)
		{
			_database = database;
		}
	}
}
