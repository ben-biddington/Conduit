using System;
using Conduit.Integration.Tests.Support;
using Conduit.UseCases.Archiving;
using NUnit.Framework;

namespace Conduit.Integration.Tests.Archiving
{
	[TestFixture]
	[Platform(Include = Platforms.Mono)]
	public class Is_not_supported_by_mono
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
			FileMachine.Make("README.md", @"Expect this to be added to the archive");

			var err = Assert.Throws<NotImplementedException>(() =>
			{
				var archive = Archive.At("Example.zip");
				Assert.IsTrue(archive.Contains("README.md"));
			});

			Assert.That(err.Message, Is.StringContaining("The requested feature is not implemented"),
				"Expected that error becaause that's what you get when something has not been ported to Mono.");
		}
	}
}