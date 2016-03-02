using System;
using System.IO;
using System.Text.RegularExpressions;
using Conduit.UseCases.Semver.Semver;
using System.Collections.Generic;

namespace Conduit.UseCases.Semver.Assemblies.Private
{
    internal static class AssemblyInfoVersion
    {
        internal static void BumpMajor(string filename, string prefix)
        {
            var newVersion = Bump.Major(Version.For.File(filename, prefix));

            Write(filename, prefix, newVersion);
        }

        internal static void BumpMinor(string filename, string prefix)
        {
            var newVersion = Bump.Minor(Version.For.File(filename, prefix));

            Write(filename, prefix, newVersion);
        }

        public static void BumpPatch(string filename, string prefix)
        {
            var newVersion = Bump.Patch(Version.For.File(filename, prefix));

            Write(filename, prefix, newVersion);
        }

        private static void Write(string filename, string prefix, SemVersion newVersion)
        {
            var lines = new List<string>();

            var pattern = new Regex(Matching.Pattern(prefix));

            foreach (var line in Lines(filename))
            {
                var match = pattern.Match(line);

                lines.Add(AssemblyInfoLine.IsInstruction(line) && match.Success
                    ? $"{match.Groups["preamble"].Value}{match.Groups["prefix"].Value}(\"{newVersion}{match.Groups["suffix"].Value}"
                    : line);
            }

            Spit(filename, string.Join(Environment.NewLine, lines));
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
            foreach (var line in File.ReadAllLines(filename)) {
                yield return line;
            }
        }
    }
}