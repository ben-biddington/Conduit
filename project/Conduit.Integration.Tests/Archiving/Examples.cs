using System;
using System.IO;
using NUnit.Framework;
using Conduit.Integration.Tests.Support;
using Conduit.UseCases.Archiving;

namespace Conduit.Integration.Tests.Archiving
{
	[TestFixture]
	[Platform(Exclude = Platforms.Mono)]
	public class Examples
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
		public void can_add_a_file()
		{
			FileMachine.Make("README.md", @"Expect this to be added to the archive");

			var archive = Archive.At("Example.zip");

			archive.Add(new FileInfo("README.md"));

			Assert.IsTrue(archive.Contains("README.md"));
		}

		[Test]
		public void can_add_a_file_and_read_it_back()
		{
			const string expected = @"Nine nine plus four pennies";

			FileMachine.Make("README.md", expected);

			var archive = Archive.At("Example.zip", new FileInfo("README.md"));

			archive.Open(new FileInfo("README.md"), s =>
			{
				using (var reader = new StreamReader(s))
				{
					Assert.AreEqual(expected, reader.ReadToEnd());
				}
			});
		}

		[Test]
		public void cannot_open_a_file_that_does_not_exist_in_the_archive()
		{
			var archive = Archive.At("Example.zip", FileMachine.Make("README.md", @"Nine nine plus four pennies"));

			var err = Assert.Throws<MissingFileError>(() => archive.Open(new FileInfo("xxx_this_files_is_not_present_xxx"), _ => {}));

			Assert.That(err.Message, Is.StringContaining("does not contain a file called <xxx_this_files_is_not_present_xxx>"));
		}

		// TEST: refuses to zip .zip files
		// TEST: refuses to zip hidden files
		// TEST: refuses to add an item if it exists -- ir should ir replace it?
	}
}

