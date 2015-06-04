using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Conduit.UseCases.Semver;
using Conduit.UseCases.Semver.Semver;

namespace Conduit.Integration.Tests.Versioning.Private
{
	internal static class AssemblyInfoVersion
	{
		private const string VERSION_PATTERN = @"\(""(?<versionstring>([^.]+).([^.]+).([^.]+)).([^.]+)""\)";

		internal static SemVersion For(string filename, string prefix)
		{
			var pattern = new Regex(string.Format("{0}{1}", prefix, VERSION_PATTERN), RegexOptions.Compiled);

			foreach (var line in File.ReadAllLines(filename).Where(it => false == string.IsNullOrEmpty(it)))
			{
				var match = pattern.Match(line);

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

		public static void BumpMajor(string filename, string prefix)
		{
			var pattern = new Regex(string.Format("{0}{1}", prefix, VERSION_PATTERN), RegexOptions.Compiled);

			var currentVersion = For(filename, prefix);

			var newVersion = Bump.Major(currentVersion);

			var replacementText = File.ReadAllText(filename);
			var replace = pattern.Replace(replacementText, string.Format(@"{0}(""{1}.*"")", prefix, newVersion));

			using (var s = File.OpenWrite(filename))
			using (var writer = new StreamWriter(s))
			{
				writer.Write(replace);
			}
		}
	}
}