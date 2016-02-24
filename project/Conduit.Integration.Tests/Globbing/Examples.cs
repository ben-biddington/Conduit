using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Conduit.Integration.Tests.Support;
using System.Linq;
using Minimatch;

namespace Conduit.Integration.Tests
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

		// TEST: consider resolving multiple matches by returning the last modified

	}

	public class Dir
	{
		public static List<string> Glob(string pattern)
		{
			return Glob (".", pattern);
		}

		public static List<string> Glob(string dir, string pattern)
		{
			var all = Directory.GetFiles (dir, "*", SearchOption.AllDirectories);

			return Minimatch.Minimatcher.Filter(all, pattern, new Options { AllowWindowsPaths = true }).ToList();
		}
	}
}

