using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Semver;

namespace Conduit.Integration.Tests.Versioning
{
	public class Can_query_assembly_version
	{
		private CleanRoom _cleanRoom;

		[SetUp]
		public void BeforeEach()
		{
			_cleanRoom = new CleanRoom(".tmp");
			_cleanRoom.Enter();
		}

		[TearDown]
		public void AfterEach()
		{
			_cleanRoom.Exit();
		}

		[Test]
		public void for_example()
		{
			FileMachine.Make("AssemblyInfo.cs", @"
				[assembly: AssemblyVersion(""1.337.0.0"")]
				[assembly: AssemblyFileVersion(""1.337.0.0"")]");

			var version = AssemblyVersion.For("AssemblyInfo.cs");

			Assert.That(version, Is.EqualTo(SemVersion.Parse("1.337.0", true)));
		}

		[Test]
		public void it_ignores_anything_after_patch_for_example_wildcard()
		{
			FileMachine.Make("AssemblyInfo.cs", @"
				[assembly: AssemblyVersion(""1.1.0.*"")]");

			var version = AssemblyVersion.For("AssemblyInfo.cs");

			Assert.That(version, Is.EqualTo(SemVersion.Parse("1.1.0", true)));
		}

		[Test]
		public void version_and_file_version_are_both_available_separately()
		{
			FileMachine.Make("AssemblyInfo.cs", @"
				[assembly: AssemblyVersion(""1.337.0.0"")]
				[assembly: AssemblyFileVersion(""1.338.0.0"")]");

			var version = AssemblyVersion.For("AssemblyInfo.cs");
			var fileVersion = AssemblyFileVersion.For("AssemblyInfo.cs");

			Assert.That(version, Is.EqualTo(SemVersion.Parse("1.337.0", true)));
			Assert.That(fileVersion, Is.EqualTo(SemVersion.Parse("1.338.0", true)));
		}

		// TEST: it fails when the file does not exist
		// TEST: it ignores anything after patch because <SemVersion> does not like it
	}

	internal static class AssemblyVersion
	{
		private const string VERSION_PATTERN = @"\(""(?<versionstring>([^.]+).([^.]+).([^.]+)).([^.]+)""\)";

		public static SemVersion For(string filename)
		{
			var lines = File.ReadAllLines(filename);

			var assemblyVersionPattern = new Regex(string.Format("AssemblyVersion{0}", VERSION_PATTERN), RegexOptions.Compiled);

			foreach (var line in lines.Where(it => false == string.IsNullOrEmpty(it)))
			{
				Match match = assemblyVersionPattern.Match(line);

				if (match.Success)
				{
					var version = match.Groups["versionstring"].Value;
					
					try
					{
						return SemVersion.Parse(version);
					}
					catch (Exception)
					{
						throw new Exception("Failed to parse ethis text to version <" + version + ">");
					}
				}
			}

			return new SemVersion(0);
		}
	}

	internal static class AssemblyFileVersion
	{
		private const string VERSION_PATTERN = @"\(""(?<versionstring>([^.]+).([^.]+).([^.]+)).([^.]+)""\)";

		public static SemVersion For(string filename)
		{
			var lines = File.ReadAllLines(filename);

			var assemblyFileVersionPattern = new Regex(string.Format("AssemblyFileVersion{0}", VERSION_PATTERN), RegexOptions.Compiled);

			foreach (var line in lines.Where(it => false == string.IsNullOrEmpty(it)))
			{
				Match match = assemblyFileVersionPattern.Match(line);

				if (match.Success)
				{
					var version = match.Groups["versionstring"].Value;

					try
					{
						return SemVersion.Parse(version);
					}
					catch (Exception)
					{
						throw new Exception("Failed to parse ethis text to version <" + version + ">");
					}
				}
			}

			return new SemVersion(0);
		}
	}

	internal static class FileMachine
	{
		public static void Make(string filename, string content)
		{
			using (var s = File.OpenWrite(filename))
			using (var writer = new StreamWriter(s))
			{
				writer.Write(content);
			}
		}
	}

	class CleanRoom
	{
		private readonly string _tempDir;
		private string _pwd;
		private string _previousDir;

		public CleanRoom(string tempDir)
		{
			_tempDir = Path.GetFullPath(tempDir);
		}

		public void Enter()
		{
			_previousDir = Directory.GetCurrentDirectory();
			_pwd = _tempDir;
			Directory.CreateDirectory(_pwd);
			Directory.SetCurrentDirectory(_pwd);
		}

		public void Exit()
		{
			Directory.SetCurrentDirectory(_previousDir);
			Directory.CreateDirectory(_pwd);
		}
	}
}
