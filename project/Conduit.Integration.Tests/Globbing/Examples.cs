using System;
using Xunit;
using System.IO;
using System.Collections.Generic;
using Conduit.Integration.Tests.Support;

namespace Conduit.Integration.Tests
{
	public class Examples : RunsInCleanRoom
	{
		[Fact]
		public void glob_single_match()
		{
			FileMachine.Touch("a.dll");
			FileMachine.Touch("a.exe");

			var result = Directory.GetFiles (".", "*.dll");

			Assert.Equal (1, result.Length);
		}

		// TEST: consider resolving multiple matches by returning the last modified

	}
}

