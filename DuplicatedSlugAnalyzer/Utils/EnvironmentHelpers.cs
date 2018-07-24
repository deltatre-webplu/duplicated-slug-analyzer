using System.IO;
using System.Reflection;

namespace DuplicatedSlugAnalyzer.Utils
{
	public class EnvironmentHelpers
	{
		public static string GetRunningAssemblyDirectoryPath()
		{
			var pathToRunningAssembly = Assembly.GetExecutingAssembly().Location;
			return Path.GetDirectoryName(pathToRunningAssembly);
		}

		public static void CreateDirectoryIfNotExisting(string directoryPath) => Directory.CreateDirectory(directoryPath);
	}
}
