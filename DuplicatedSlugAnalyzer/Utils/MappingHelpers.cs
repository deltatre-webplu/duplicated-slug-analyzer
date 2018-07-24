using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Deltatre.Utils.Extensions.Dictionary;
using DuplicatedSlugAnalyzer.Guishell;

namespace DuplicatedSlugAnalyzer.Utils
{
	public static class MappingHelpers
	{
		public static ReadOnlyDictionary<string, string> GetEntityCodeToDistributionCodeMap(
			IEnumerable<CustomEntity> customEntities)
		{
			if (customEntities == null)
				throw new ArgumentNullException(nameof(customEntities));

			return customEntities
				.ToDictionary(
					ce => ce.Code,
					ce => ce.DistributionCode)
				.AsReadOnly();
		}
	}
}
