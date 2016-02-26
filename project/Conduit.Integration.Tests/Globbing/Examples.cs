using System;
using System.IO;
using System.Linq;
using System.Threading;
using Conduit.Integration.Tests.Support;
using Xunit;

namespace Conduit.Integration.Tests.Globbing
{
    public class Examples : RunsInCleanRoom
    {
        [Fact]
        public void find_the_newest_file_by_name()
        {
            FileMachine.Touch("bin", "Debug", "example.dll");

            Thread.Sleep(TimeSpan.FromMilliseconds(500));

            var expected = FileMachine.Touch("bin", "Release", "example.dll");

            var actual = Dir.Newest("example.dll");

            Assert.Equal(expected.FullName, actual.FullName);
        }
    }

    public class Dir
    {
        public static FileInfo Newest(string filenameWithExtension)
        {
            var newest = Directory.GetFiles(Environment.CurrentDirectory, $"*{filenameWithExtension}", SearchOption.AllDirectories).Select(it => new FileInfo(it)).ToList();

            if (false == newest.Any())
                return null;

            return newest.OrderBy(it => it.LastWriteTimeUtc).LastOrDefault();
        }
    }
}

