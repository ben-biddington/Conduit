using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Conduit.UseCases.Semver;
using Conduit.UseCases.Semver.Semver;
using System.Collections.Generic;
using Conduit.UseCases.Semver.Assemblies.Private;

namespace Conduit.UseCases.Semver.Assemblies.Private.Version
{
    internal static class For
    {
        internal static SemVersion File(string filename, string prefix)
        {
            var pattern = new Regex(Matching.Pattern(prefix), RegexOptions.Compiled | RegexOptions.ExplicitCapture);

            foreach (var line in Lines(filename)) {
                var match = pattern.Match(line);

                if (match.Success)
                    return SemVersionFrom(filename, Matching.Pattern(prefix), match);
            }

            return new SemVersion (0);
        }

        private static SemVersion SemVersionFrom(string filename, string pattern, Match match)
        {
            var version = match.Groups ["versionstring"].Value;

            try {
                return SemVersion.Parse (version);
            } catch (Exception) {
                throw new Exception($"Failed to parse this text to version <{version}>. Pattern: <{pattern}>, File: <{filename}>");
            }
        }

        private static IEnumerable<string> Lines(string filename)
        {
            foreach (var line in System.IO.File.ReadAllLines(filename).Where(AssemblyInfoLine.IsInstruction)) {
                yield return line;
            }
        }
    }
}