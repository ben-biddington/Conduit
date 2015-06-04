using Conduit.UseCases.Semver;
using Conduit.UseCases.Semver.Semver;
using NUnit.Framework;

namespace Conduit.Unit.Tests.SemanticVersioning
{
    public class Can_increment_major
    {
        [Test]
        public void like_this()
        {
            var versionOne = new SemVersion(1);

            var versionTwo = Bump.Major(versionOne);

            Assert.That(versionTwo.Major, Is.EqualTo(2));
        }

        [Test]
        public void and_it_leaves_the_original_value_unchanged()
        {
            var versionOne = new SemVersion(1);

            Bump.Major(versionOne);

            Assert.That(versionOne.Major, Is.EqualTo(1));
        }
    }
}
