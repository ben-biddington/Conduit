using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Conduit.UseCases.Semver;
using Conduit.UseCases.Semver.Semver;
using System.Collections.Generic;

namespace Conduit.Integration.Tests.Versioning.Private
{
	internal static class AssemblyInfoVersion
	{
		public static void BumpMajor(string filename, string prefix)
		{
			var newVersion = Bump.Major(For (filename, prefix));

			var lines = new List<string>();

			var pattern = new Regex(Matching.Pattern(prefix));

			foreach (var line in Lines(filename))
			{
				var match = pattern.Match(line);

				if (match.Success)
				{
					var newVersionLine = string.Format("{0}{1}(\"{2}{3}", match.Groups["preamble"].Value, match.Groups["prefix"].Value, newVersion, match.Groups["suffix"].Value);

					lines.Add(newVersionLine);
				} else {
					lines.Add(line);
				}
			}

			Spit(filename, string.Join (Environment.NewLine, lines));
		}

		private static void Spit(string filename, string content)
		{
			using (var s = File.Open(filename, FileMode.Truncate))
			using (var writer = new StreamWriter(s))
			{
				writer.Write(content);
			}
		}

		internal static SemVersion For(string filename, string prefix)
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
			foreach (var line in File.ReadAllLines(filename).Where(AssemblyInfoLine.IsInstruction))
			{
				yield return line;
			}
		}
	}
}