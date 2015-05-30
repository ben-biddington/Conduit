using System;
using NUnit.Framework;

namespace Conduit.Unit.Tests
{
	public class Can_increment_versions
	{
		public Can_increment_versions ()
		{
		}

		[Test]
		public void for_example_you_may_bump_major_version() {
			var semver = new Semver(0);

			var bumped = semver.BumpMajor();

			Assert.AreEqual (0, semver.Major, "Expected Semver to be a value: must be immutable");
			Assert.AreEqual (1, bumped.Major);
		}
	}

	class Semver {
		public int Major { get; private set;}

		public Semver(int major) {
			Major = major;
		}

		public Semver BumpMajor() {
			return new Semver(Major + 1);
		}
	}
}

