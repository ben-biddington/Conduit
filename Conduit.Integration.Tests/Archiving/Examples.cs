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
		public void the_basics() 
		{
			FileMachine.Make("README.md", @"Expect this to be added to the archive");

			var archive = Archive.At("Example.zip");

			Assert.That(archive.Contains("README.md"));
		}
	}	
}

