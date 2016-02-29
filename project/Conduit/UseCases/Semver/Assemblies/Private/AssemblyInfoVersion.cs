using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Conduit.UseCases.Semver;
using Conduit.UseCases.Semver.Semver;
using System.Collections.Generic;
using Conduit.UseCases.Semver.Assemblies.Private;

namespace Conduit.UseCases.Semver.Assemblies.Private
{
    internal static class AssemblyInfoVersion
    {
        internal static void BumpMajor(string filename, string prefix)
        {
            var newVersion = Bump.Major(Version.For.File(filename, prefix));

            var lines = new List<string> ();

            var pattern = new Regex(Matching.Pattern(prefix));

            foreach (var line in Lines(filename)) {
                var match = pattern.Match (line);

                lines.Add (match.Success 
                    ? string.Format ("{0}{1}(\"{2}{3}", match.Groups ["preamble"].Value, match.Groups ["prefix"].Value, newVersion, match.Groups ["suffix"].Value) 
                    : line);
            }

            Spit(filename, string.Join (Environment.NewLine, lines));
        }

        internal static void BumpMinor(string filename, string prefix)
        {
            var newVersion = Bump.Minor(Version.For.File(filename, prefix));

            var lines = new List<string> ();

            var pattern = new Regex (Matching.Pattern (prefix));

            foreach (var line in Lines(filename)) {
                var match = pattern.Match (line);

                lines.Add (match.Success 
                    ? string.Format ("{0}{1}(\"{2}{3}", match.Groups ["preamble"].Value, match.Groups ["prefix"].Value, newVersion, match.Groups ["suffix"].Value) 
                    : line);
            }

            Spit(filename, string.Join (Environment.NewLine, lines));
        }

        private static void Spit(string filename, string content)
        {
            using (var s = File.Open (filename, FileMode.Truncate))
            using (var writer = new StreamWriter (s)) {
                writer.Write (content);
            }
        }

        private static IEnumerable<string> Lines(string filename)
        {
            foreach (var line in File.ReadAllLines(filename).Where(AssemblyInfoLine.IsInstruction)) {
                yield return line;
            }
        }
    }
}