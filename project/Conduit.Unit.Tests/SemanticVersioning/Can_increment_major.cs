using Conduit.UseCases.Semver;
using Conduit.UseCases.Semver.Semver;
using Xunit;

namespace Conduit.Unit.Tests.SemanticVersioning
{
    public class Can_increment_major
    {
        [Fact]
        public void like_this()
        {
            var versionOne = new SemVersion(1);

            var versionTwo = Bump.Major(versionOne);

            Assert.True(versionTwo.Major.Equals(2));
        }

        [Fact]
        public void and_it_leaves_the_original_value_unchanged()
        {
            var versionOne = new SemVersion(1);

            Bump.Major(versionOne);

			Assert.True(versionOne.Major.Equals(1));
        }
    }
}
