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
				// Version information for an assembly consists of the following four values:
				//
				//      Major Version
				//      Minor Version 
				//      Build Number
				//      Revision
				//
				// You can specify all the values or you can default the Build and Revision Numbers 
				// by using the '*' as shown below:
				// [assembly: AssemblyVersion(""1.0.*"")]
				[assembly: AssemblyVersion(""1.337.0.0"")]
				[assembly: AssemblyFileVersion(""1.337.0.0"")]");

			var version = AssemblyVersion.For("AssemblyInfo.cs");

			Assert.That(version, Is.EqualTo(SemVersion.Parse("1.337.0", true)));
		}

		// TEST: it fails when the file does not exist
	}

	internal static class AssemblyVersion
	{
		public static SemVersion For(string filename)
		{
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
