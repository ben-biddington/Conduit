using Conduit.Integration.Tests.Support;
using Conduit.UseCases.Semver.Semver;
using NUnit.Framework;

namespace Conduit.Integration.Tests.Versioning
{
	public class Can_bump_an_assembly_version
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
				// [assembly: AssemblyVersion(""1.0.*"")]
				[assembly: AssemblyVersion(""0.0.0.*"")]
				[assembly: AssemblyFileVersion(""0.0.0.*"")]");

			AssemblyVersion.BumpMajor("AssemblyInfo.cs");

			var version = AssemblyVersion.For("AssemblyInfo.cs");

			Assert.That(version, Is.EqualTo(SemVersion.Parse("1.0.0", true)));
		}
	}
}