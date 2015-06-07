using System;
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
		public void works_on_windows()
		{
			FileMachine.Make("README.md", @"Expect this to be added to the archive");

			var archive = Archive.At("Example.zip");

			Assert.IsTrue(archive.Contains("README.md"));
		}
	}
}

