using Conduit.Integration.Tests.Support;
using Conduit.UseCases.Semver.Semver;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using Conduit.UseCases.Semver.Assemblies;

namespace Conduit.Integration.Tests.Support
{
    internal static class TextFile
    {
        internal static bool Contains(string filename, string expected)
        {
            foreach (var line in File.ReadAllLines(filename).Where(it => false == string.IsNullOrEmpty(it))) {
                if (line.Contains (expected))
                    return true;
            }

            return false;
        }

        internal static bool LinesEqual(string filename, string expected)
        {
            var expectedLines = expected.Split(Environment.NewLine.ToCharArray()).ToList();

            var actual = File.ReadAllLines(filename).ToList();

            return Enumerable.SequenceEqual<string>(expectedLines, actual);
        }
    }
}