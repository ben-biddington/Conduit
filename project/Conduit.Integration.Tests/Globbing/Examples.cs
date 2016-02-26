using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;
using Conduit.Integration.Tests.Support;
using Minimatch;
using Xunit;

namespace Conduit.Integration.Tests.Globbing
{
	public class Examples : RunsInCleanRoom
	{
		[Fact]
		public void glob_single_match()
		{
			FileMachine.Touch("a.dll");
			FileMachine.Touch("a.exe");

			var result = Dir.Glob(Path.Combine(".", "*.dll"));

			Assert.Equal (1, result.Count);
		}

		[Fact]
		public void glob_match_files_in_directories()
		{
			FileMachine.Touch("bin", "Debug"	, "example.dll");
			FileMachine.Touch("bin", "Release"	, "example.dll");
			FileMachine.Touch("example.dll");

			var result = Dir.Glob(Path.Combine(".", "bin", "*", "*.dll"));

			Assert.Equal (2, result.Count);
		}

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

	    public static List<string> Glob(string pattern)
		{
			return Glob (".", pattern);
		}

		public static List<string> Glob(string dir, string pattern)
		{
			var all = Directory.GetFiles (dir, "*", SearchOption.AllDirectories);

			return Minimatcher.Filter(all, pattern, new Options { AllowWindowsPaths = true }).ToList();
		}
	}
}

