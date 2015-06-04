using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Conduit.UseCases.Semver.Semver;

namespace Conduit.Integration.Tests.Versioning.Private
{
	internal static class AssemblyInfoVersion
	{
		private const string VERSION_PATTERN = @"\(""(?<versionstring>([^.]+).([^.]+).([^.]+)).([^.]+)""\)";

		public static SemVersion For(string filename, string prefix)
		{
			var assemblyFileVersionPattern = new Regex(string.Format("{0}{1}",prefix, VERSION_PATTERN), RegexOptions.Compiled);

			foreach (var line in File.ReadAllLines(filename).Where(it => false == string.IsNullOrEmpty(it)))
			{
				var match = assemblyFileVersionPattern.Match(line);

				if (match.Success)
				{
					return SemVersionFrom(match);
				}
			}

			return new SemVersion(0);
		}

		private static SemVersion SemVersionFrom(Match match)
		{
			var version = match.Groups["versionstring"].Value;

			try
			{
				return SemVersion.Parse(version);
			}
			catch (Exception)
			{
				throw new Exception("Failed to parse this text to version <" + version + ">");
			}
		}
	}
}