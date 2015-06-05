using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Conduit.UseCases.Semver;
using Conduit.UseCases.Semver.Semver;
using System.Collections.Generic;
using Conduit.Integration.Tests.Versioning.Private;

namespace Conduit.Integration.Tests.Versioning.AssemblyInfoVersion
{
	public static class For 
	{
		public static SemVersion File(string filename, string prefix)
		{
			var pattern = new Regex(Matching.Pattern(prefix), RegexOptions.Compiled);

			foreach (var line in Lines(filename))
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

		private static IEnumerable<string> Lines(string filename) {
			foreach (var line in System.IO.File.ReadAllLines(filename).Where(AssemblyInfoLine.IsInstruction))
			{
				yield return line;
			}
		}
	}
}