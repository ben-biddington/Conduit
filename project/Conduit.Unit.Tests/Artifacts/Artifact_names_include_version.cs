using Conduit.Artifacts;
using Conduit.UseCases.Semver.Semver;
using Xunit;

namespace Conduit.Unit.Tests.Artifacts
{
    // TEST: artifact names look like `Assembly.Name-1.337.0-master`

    public class Examples
    {
        [Fact]
        public void one()
        {
            var name = new ArtifactName("Assembly.Name", new SemVersion(0,0,1, string.Empty, "429"), new SourceControlBranch("f/example-with-hyphens"));

            Assert.True("Assembly.Name-f_example_with_hyphens-v0.0.1.429".Equals(name.ToString()), $"Expected Artifact name to equal <Assembly.Name-f_example_with_hyphens-v0.0.1.429>, but it is actually <{name}>.");
        } 
    }

    public class Artifact_names_include_version
    {
        [Fact]
        public void for_example()
        {
            var version = new SemVersion(1);

            var name = new ArtifactName("Assembly.Name", version, SourceControlBranch.Master);

            Assert.True(name.ToString().Contains("1.0.0"));
        }
    }

    public class Artifact_names_include_source_control_branch_name
    {
        [Fact]
        public void for_example()
        {
            var branchName = new SourceControlBranch("next");

            var name = new ArtifactName("Assembly.Name", new SemVersion(1), branchName);

            Assert.True(name.ToString().Contains("next"));
        }
    }
}

