using Conduit.UseCases.Semver.Semver;

namespace Conduit.UseCases.Semver
{
    public static class Bump
    {
        public static SemVersion Major(SemVersion version)
        {
            return new SemVersion(version.Major + 1);
        }
    }
}