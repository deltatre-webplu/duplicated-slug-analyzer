﻿using System;
using System.Collections.Generic;
using LanguageExt;
using MongoDB.Bson;
using MongoDB.Driver;
using static LanguageExt.Option<MongoDB.Driver.IMongoCollection<MongoDB.Bson.BsonDocument>>;

namespace DuplicatedSlugAnalyzer.Distribution
{
	public class DistributionCollectionFactory
	{
		private readonly IMongoDatabase _distributionDatabase;
		private readonly IReadOnlyDictionary<string, string> _entityCodeToDistributionCode;
		private const string CollectionNameSeparator = "__";

		public DistributionCollectionFactory(
			IMongoDatabase distributionDatabase,
			IReadOnlyDictionary<string, string> entityCodeToDistributionCode)
		{
			_distributionDatabase = distributionDatabase 
			                        ?? throw new ArgumentNullException(nameof(distributionDatabase));
			_entityCodeToDistributionCode = entityCodeToDistributionCode 
			                                ?? throw new ArgumentNullException(nameof(entityCodeToDistributionCode));
		}

		public Option<IMongoCollection<BsonDocument>> GetDistributionResourceCollection(
			string culture,
			string entityType,
			string entityCode)
		{
			var collectionName = BuildCollectionName(
				culture,
				entityType,
				entityCode);

			return collectionName.Match(
				cn => Some(
					_distributionDatabase.GetCollection<BsonDocument>(
						cn, new MongoCollectionSettings
						{
							GuidRepresentation = GuidRepresentation.CSharpLegacy
						}
					)
				),
				() => None);
		}

		private Option<string> BuildCollectionName(
			string culture,
			string entityType,
			string entityCode)
		{
			var resourceName = GetDistributionResourceName(entityType, entityCode);

			return resourceName.Match(
				rn => $"{culture}{CollectionNameSeparator}{rn}",
				() => Option<string>.None);
		}

		private Option<string> GetDistributionResourceName(
			string entityType,
			string entityCode)
		{
			switch (entityType.ToLowerInvariant())
			{
				case "album":
					return "albums";

				case "photo":
					return "photos";

				case "document":
					return "documents";

				case "story":
					return "stories";

				case "tag":
					return "tags";

				case "customentity":
					return LookupDistributionCode(entityCode);

				case "selection":
					return Option<string>.None;

				default:
					throw new UnknownEntityTypeException($"Unknown entity type: '{entityType}'.");
			}
		}

		private string LookupDistributionCode(string entityCode)
		{
			var distributionCodeFound = _entityCodeToDistributionCode.TryGetValue(
				entityCode,
				out var distributionCode);
			if (!distributionCodeFound)
				throw new DistributionCodeNotFoundException($"Unable to find distribution code corresponding to entity code '{entityCode}'.");

			return distributionCode;
		}
	}
}
