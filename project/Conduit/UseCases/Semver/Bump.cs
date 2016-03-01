using Conduit.UseCases.Semver.Semver;

namespace Conduit.UseCases.Semver
{
    public static class Bump
    {
        public static SemVersion Major(SemVersion version)
        {
            return new SemVersion(version.Major + 1);
        }

        public static SemVersion Minor(SemVersion version)
        {
            return new SemVersion(version.Major, version.Minor + 1);
        }

        public static SemVersion Patch(SemVersion version)
        {
            return new SemVersion(version.Major, version.Minor, version.Patch + 1);
        }
    }
}