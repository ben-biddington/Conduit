using Conduit.UseCases.Semver.Semver;

namespace Conduit.Artifacts
{
    public class ArtifactName
    {
        private readonly SemVersion _version;
        private readonly SourceControlBranch _branch;

        public ArtifactName(SemVersion version, SourceControlBranch branch)
        {
            _version = version;
            _branch = branch;
        }

        public override string ToString()
        {
            return string.Join(string.Empty, _version.ToString(), _branch.Name);
        }
    }
}