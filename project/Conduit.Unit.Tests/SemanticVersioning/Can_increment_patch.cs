using Conduit.UseCases.Semver;
using Conduit.UseCases.Semver.Semver;
using Xunit;

namespace Conduit.Unit.Tests.SemanticVersioning
{
    public class Can_increment_patch
    {
        [Fact]
        public void like_this()
        {
            var versionOne = new SemVersion(1,1,1);

            var versionTwo = Bump.Patch(versionOne);

            var expected  = new SemVersion(1,1,2);

            Assert.True(versionTwo.Equals(expected ), $"Expected <{expected}>, got <{versionTwo}>");
        }
    }
}