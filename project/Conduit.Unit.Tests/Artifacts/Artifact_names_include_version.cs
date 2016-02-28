using Conduit.UseCases.Semver.Semver;
using Xunit;

namespace Conduit.Unit.Tests.Artifacts
{
    // TEST: artifact names look like `Assembly.Name-1.337.0-master`

    public class Artifact_names_include_version
    {
        [Fact]
        public void for_example()
        {
            var version = new SemVersion(1);

            var name = new ArtifactName(version, SourceControlBranch.Master);

            Assert.True(name.ToString().Contains("1.0.0"));
        }
    }

    public class Artifact_names_include_source_control_branch_name
    {
        [Fact]
        public void for_example()
        {
            var branchName = new SourceControlBranch("next");

            var name = new ArtifactName(new SemVersion(1), branchName);

            Assert.True(name.ToString().Contains("next"));
        }
    }

    class SourceControlBranch
    {
        public static readonly SourceControlBranch Master = new SourceControlBranch("master");
        public string Name { get; private set; }

        internal SourceControlBranch(string name)
        {
            Name = name;
        }
    }

    class ArtifactName
    {
        private readonly SemVersion _version;
        private readonly SourceControlBranch _branch;

        internal ArtifactName(SemVersion version, SourceControlBranch branch)
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

