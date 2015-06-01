using NUnit.Framework;
using Semver;

namespace Conduit.Unit.Tests.SemanticVersioning
{
    [TestFixture]
    public class Can_increment_major
    {
        [Test]
        public void like_this()
        {
            var versionOne = new SemVersion(1);

            var versionTwo = BumpMajor(versionOne);

            Assert.That(versionTwo.Major, Is.EqualTo(2));
        }

        private SemVersion BumpMajor(SemVersion versionOne)
        {
            return new SemVersion(versionOne.Major + 1);
        }

        // TEST: it leaves the value unchanged
    }
}
