using System.IO;
using System.Linq;
using Conduit.UseCases.Archiving;
using Conduit.Integration.Tests.Archiving.Support;
using Conduit.Integration.Tests.Support;
using Xunit;

namespace Conduit.Integration.Tests.Archiving
{
	public class Can_make_archive_from_entire_directory : RunsInCleanRoom
	{
		[Fact]
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

		[Fact]
		public void and_it_is_recursive()
		{
			FileMachine.Make("A\\A.txt", "A");
			FileMachine.Make("A\\B\\B.txt", "A");
			FileMachine.Make("A\\B\\C\\C.txt", "A");

			var archive = Archive.At("Example.zip", new DirectoryInfo("A"));

			archive.MustContain("A.txt");
			archive.MustContain("B\\B.txt");
			archive.MustContain("B\\C\\C.txt");
		}

		// TEST: paths inside MUST match the operation system (?)
	}

	//[Platform(Exclude = Platforms.Mono)]
	public class Can_make_an_empty_one: RunsInCleanRoom
	{
		[Fact]
		public void for_example()
		{
			var archive = Archive.At("Example.zip");

			Assert.Equal(0, archive.Contents().Count());
		}
	}
}