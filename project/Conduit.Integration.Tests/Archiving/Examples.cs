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

			var archive = Archive.At("Example.zip");

			archive.Add(new FileInfo("README.md"));

			archive.Open(new FileInfo("README.md"), s =>
			{
				using (var reader = new StreamReader(s))
				{
					var allText = reader.ReadToEnd();
					Assert.AreEqual(expected, allText);
				}
			});
		}

		// TEST: opening bung file fails
		// TEST: refuses to zip .zip files
		// TEST: refuses to zip hidden files
		// TEST: refuses to add an item if it exists -- ir should ir replace it?
	}
}

