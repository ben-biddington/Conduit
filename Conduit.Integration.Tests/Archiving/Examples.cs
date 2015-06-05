using System;
using NUnit.Framework;
using Conduit.Integration.Tests.Support;
using Conduit.UseCases.Archiving;

namespace Conduit.Integration.Tests.Archiving
{
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
		public void cannot_work_on_mono() 
		{
			FileMachine.Make("README.md", @"Expect this to be added to the archive");

			var archive = Archive.At ("Example.zip");

			var err = Assert.Throws<NotImplementedException>(() => archive.Contains("README.md"));

			Assert.That(err.Message, Is.StringMatching("The requested feature is not implemented"), "This will fail on mono as it has yet to be implemented");
		}
	}	
}

