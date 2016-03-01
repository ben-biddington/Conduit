using System;
using System.Linq;
using System.IO;

namespace Conduit.Integration.Tests.Support
{
    internal static class TextFile
    {
        internal static bool Contains(string filename, string expected)
        {
            return File.ReadAllLines(filename).Where(it => false == string.IsNullOrEmpty(it)).Any(line => line.Contains(expected));
        }

        internal static bool LinesEqual(string filename, string expected)
        {
            var charArray = (expected.Contains("\r\n") ? "\r\n" : "\n").ToCharArray();

            var expectedLines = expected.Split(charArray, StringSplitOptions.RemoveEmptyEntries).ToList();

            var actual = File.ReadAllLines(filename).ToList().Where(it => false == string.IsNullOrEmpty(it));

            return expectedLines.SequenceEqual(actual);
        }
    }
}