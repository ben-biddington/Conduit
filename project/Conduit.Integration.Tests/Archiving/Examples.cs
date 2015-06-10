using System.IO;
using NUnit.Framework;
using Conduit.Integration.Tests.Support;
using Conduit.UseCases.Archiving;

namespace Conduit.Integration.Tests.Archiving
{
	[TestFixture]
	public class Examples : RunsInCleanRoom
	{
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

		[Test]
		public void cannot_add_a_file_that_does_not_exist_on_disk()
		{
			var err = Assert.Throws<IOException>(() => Archive.At("Example.zip", new FileInfo("xxx_does_not_exist_xxx")));

			Assert.That(err.Message, Is.StringContaining("Cannot add a file that does not exist"));
		}

		[Test]
		public void can_archive_zip_files()
		{
			var documents = Archive.At("documents.zip", FileMachine.Make("1.html", "An example proving that you may zip a zip file"));

			Assert.DoesNotThrow(() => Archive.At("Example.zip", documents.Filename));
		}

		// TEST: Can add a directory ignoring a glob pattern
		// TEST: why is there an xml file called <[Content_Types].xml> in every archive? Seems to be required, otherwise files are not located
		// TEST: refuses to add an item if it exists -- replace it?
	}
}
