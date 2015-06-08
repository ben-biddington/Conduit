using Conduit.UseCases.Archiving;
using NUnit.Framework;

namespace Conduit.Integration.Tests.Support
{
	public static class ArchiveAssertions
	{
		public static void MustContain(this Archive self, params string[] names)
		{
			foreach (var name in names)
			{
				Assert.IsTrue(self.Contains(name), "Expected the archive to contain <{0}>", name);
			}
		}

		public static void MustNotContain(this Archive self, params string[] names)
		{
			foreach (var name in names)
			{
				Assert.IsFalse(self.Contains(name), "Expected the archive to NOT contain <{0}>", name);
			}
		}
	}
}