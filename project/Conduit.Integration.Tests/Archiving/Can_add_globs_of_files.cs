using System.IO;
using System.Linq;
using Conduit.Integration.Tests.Support;
using Conduit.UseCases.Archiving;
using NUnit.Framework;

namespace Conduit.Integration.Tests.Archiving
{
	[TestFixture]
	[Platform(Exclude = Platforms.Mono)]
	public class Can_add_globs_of_files
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
			FileMachine.Make("A.txt", "A");
			FileMachine.Make("B.txt", "A");
			FileMachine.Make("C.txt", "A");

			var archive = Archive.At("Example.zip", Files.Like("*.txt"));

			archive.MustContain("A.txt");
		}
	}

	public static class ArchiveAssertions
	{
		public static void MustContain(this Archive self, params string[] names)
		{
			foreach (var name in names)
			{
				Assert.IsTrue(self.Contains(name), "Expected the archive to contain <{0}>", name);
			}
		}
	}

	internal class Files
	{
		public static FileInfo[] Like(string txt)
		{
			return Directory.GetFiles(Directory.GetCurrentDirectory(), txt).Select(it => 
				new FileInfo(Path.GetFullPath(it))).ToArray();
		}
	}
}