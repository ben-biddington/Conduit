using System.IO;
using Conduit.Integration.Tests.Support;
using Conduit.UseCases.Semver.Semver;
using NUnit.Framework;
using Conduit.UseCases.Semver.Assemblies;

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
				using System.Reflection;
				using System.Runtime.CompilerServices;
				using System.Runtime.InteropServices;

				// General Information about an assembly is controlled through the following 
				// set of attributes. Change these attribute values to modify the information
				// associated with an assembly.
				[assembly: AssemblyTitle(""Conduit.Integration.Tests"")]
				[assembly: AssemblyDescription("""")]
				[assembly: AssemblyConfiguration("""")]
				[assembly: AssemblyCompany("""")]
				[assembly: AssemblyProduct(""Conduit.Integration.Tests"")]
				[assembly: AssemblyCopyright("""")]
				[assembly: AssemblyTrademark("""")]
				[assembly: AssemblyCulture("""")]	

				// Setting ComVisible to false makes the types in this assembly not visible 
				// to COM components.  If you need to access a type in this assembly from 
				// COM, set the ComVisible attribute to true on that type.
				[assembly: ComVisible(false)]

				// The following GUID is for the ID of the typelib if this project is exposed to COM
				[assembly: Guid(""278c86e3-1ac7-4a3f-a0c6-f1e072422836"")]

				// Version information for an assembly consists of the following four values:
				//
				//      Major Version
				//      Minor Version 
				//      Build Number
				//      Revision
				//
				// You can specify all the values or you can default the Build and Revision Numbers 
				// by using the '*' as shown below:
				[assembly: AssemblyVersion(""1.337.0.0"")]
				[assembly: AssemblyFileVersion(""1.337.0.0"")]");

			var version = AssemblyVersion.For("AssemblyInfo.cs");

			Assert.That(version, Is.EqualTo(SemVersion.Parse("1.337.0")));
		}

		[Test]
		public void it_ignores_anything_after_patch_for_example_wildcard()
		{
			FileMachine.Make("AssemblyInfo.cs", @"
				[assembly: AssemblyVersion(""1.1.0.*"")]");

			var version = AssemblyVersion.For("AssemblyInfo.cs");

			Assert.That(version, Is.EqualTo(SemVersion.Parse("1.1.0")));
		}

		[Test]
		public void version_and_file_version_are_both_available_separately()
		{
			FileMachine.Make("AssemblyInfo.cs", @"
				[assembly: AssemblyVersion(""1.337.0.0"")]
				[assembly: AssemblyFileVersion(""1.338.0.0"")]");

			var version = AssemblyVersion.For("AssemblyInfo.cs");
			var fileVersion = AssemblyFileVersion.For("AssemblyInfo.cs");

			Assert.That(version, Is.EqualTo(SemVersion.Parse("1.337.0")));
			Assert.That(fileVersion, Is.EqualTo(SemVersion.Parse("1.338.0")));
		}

		[Test]
		public void it_fails_when_file_does_not_exist()
		{
			FileMachine.Make("MisnamedFile.cs", @"
				[assembly: AssemblyVersion(""1.1.0.*"")]");

			Assert.Throws<FileNotFoundException>(() => AssemblyVersion.For("AssemblyInfo.cs"));
		}

		[Test]
		public void it_returns_blank_version_when_nothing_matches()
		{
			FileMachine.Make("AssemblyInfo.cs", @"// Just other stuff, no version attributes");

			var version = AssemblyVersion.For("AssemblyInfo.cs");

			Assert.That(version, Is.EqualTo(SemVersion.Parse("0.0.0")));
		}

		// TEST: it cannot handle wildcards like: [assembly: AssemblyVersion("1.0.*")]
	}
}
