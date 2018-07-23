using static  System.IO.Directory;
using static System.Reflection.Assembly;
using static System.IO.Path;

namespace DuplicatedSlugAnalyzer.Helpers
{
	public class EnvironmentHelpers
	{
		public static string GetRunningAssemblyDirectoryPath()
		{
			var pathToRunningAssembly = GetExecutingAssembly().Location;
			return GetDirectoryName(pathToRunningAssembly);
		}

		public static void CreateDirectoryIfNotExisting(string directoryPath) => CreateDirectory(directoryPath);
	}
}
