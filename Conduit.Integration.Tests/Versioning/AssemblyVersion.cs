using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Semver;

namespace Conduit.Integration.Tests.Versioning
{
	internal static class AssemblyVersion
	{
		private const string VERSION_PATTERN = @"\(""(?<versionstring>([^.]+).([^.]+).([^.]+)).([^.]+)""\)";

		public static SemVersion For(string filename)
		{
			var lines = File.ReadAllLines(filename);

			var assemblyVersionPattern = new Regex(string.Format("AssemblyVersion{0}", VERSION_PATTERN), RegexOptions.Compiled);

			foreach (var line in lines.Where(it => false == string.IsNullOrEmpty(it)))
			{
				Match match = assemblyVersionPattern.Match(line);

				if (match.Success)
				{
					var version = match.Groups["versionstring"].Value;
					
					try
					{
						return SemVersion.Parse(version);
					}
					catch (Exception)
					{
						throw new Exception("Failed to parse ethis text to version <" + version + ">");
					}
				}
			}

			return new SemVersion(0);
		}
	}
}