using System.Text.RegularExpressions;
using Conduit.UseCases.Semver.Semver;

namespace Conduit.Unit.Tests.Artifacts
{
    class ArtifactName
    {
        private readonly string _name;
        private readonly SemVersion _version;
        private readonly SourceControlBranch _branch;

        internal ArtifactName(string name, SemVersion version, SourceControlBranch branch)
        {
            _name = name;
            _version = version;
            _branch = branch;
        }

        public override string ToString()
        {
            return string.Join("-", _name, Sanitise(_branch.Name), $"v{_version}");
        }

        private string Sanitise(string value) => Regex.Replace(value, @"[\W]", "_");
    }
}