using System;
using Conduit.UseCases.Archiving;
using Xunit;

namespace Conduit.Integration.Tests.Support
{
	public static class ArchiveAssertions
	{
		public static void MustContain(this Archive self, params string[] names)
		{
			foreach (var name in names)
			{
				Assert.True(self.Contains(name), "Expected the archive to contain <" + name + " >");
			}
		}

		public static void MustNotContain(this Archive self, params string[] names)
		{
			foreach (var name in names)
			{
				Assert.False(self.Contains(name), 
					"Expected the archive to NOT contain <" + name + ">. It contains these files:\r\n\r\n" + string.Join(Environment.NewLine, self.Contents()) + " \r\n");
			}
		}
	}
}