using System.IO;
using System.Linq;
using Conduit.Integration.Tests.Archiving.Support;
using Conduit.Integration.Tests.Support;
using Conduit.UseCases.Archiving;
using NUnit.Framework;

namespace Conduit.Integration.Tests.Archiving
{
	[TestFixture]
	[Platform(Exclude = Platforms.Mono)]
	public class Can_make_archive_from_entire_directory : RunsInCleanRoom
	{
		[Test]
		public void for_example()
		{
			FileMachine.Make("A\\A.txt", "A");
			FileMachine.Make("A\\B.txt", "A");
			FileMachine.Make("A\\C.txt", "A");
			FileMachine.Make("A\\D.html", "A");

			var archive = Archive.At("Example.zip", new DirectoryInfo("A"));

			archive.MustContain("A.txt");
			archive.MustContain("B.txt");
			archive.MustContain("C.txt");
			archive.MustContain("D.html");
		}
	}

	[TestFixture]
	[Platform(Exclude = Platforms.Mono)]
	public class Can_make_an_empty_one: RunsInCleanRoom
	{
		[Test]
		public void for_example()
		{
			var archive = Archive.At("Example.zip");

			Assert.AreEqual(0, archive.Contents().Count());
		}
	}
}