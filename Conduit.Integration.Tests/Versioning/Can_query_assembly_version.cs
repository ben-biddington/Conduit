using System.IO;
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

		[Test]
		public void it_fails_when_file_does_not_exist()
		{
			FileMachine.Make("MisnamedFile.cs", @"
				[assembly: AssemblyVersion(""1.1.0.*"")]");

			Assert.Throws<FileNotFoundException>(() => AssemblyVersion.For("AssemblyInfo.cs"));
		}
	}
}
