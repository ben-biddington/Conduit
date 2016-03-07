using System.IO;
using Conduit.Adapters.Build.Packaging;
using Conduit.Integration.Tests.Support;
using Xunit;

namespace Conduit.Integration.Tests.Packaging.Flattening
{
    public class SamplePackages
    {
        public DirectoryInfo PackagesDir { get; }
        public NugetPackage[] Packages { get; }

        public SamplePackages()
        {
            Packages = new[]
            {
                new NugetPackage("Minimatch", new PackageVersion("1.1.0"), FrameworkVersion.Net45),
                new NugetPackage("Conduit.Build.Targets", new PackageVersion("0.2.2"), FrameworkVersion.Net45)
            };

            PackagesDir = new DirectoryInfo("packages");

            Nuget.Install(Settings.PublicNuget, PackagesDir, Packages);
        }
    }

    public class Can_query_local_package_repository : IClassFixture<SamplePackages>
    {
        private readonly SamplePackages _samples;

        public Can_query_local_package_repository(SamplePackages samples)
        {
            _samples = samples;
        }

        [Fact]
        public void for_example()
        {
            var actual = LocalPackages.Find(Settings.PublicNuget, _samples.PackagesDir, "Conduit.Build.Targets");

            Assert.True(actual.FrameworkVersion.Equals(FrameworkVersion.Net45));
            Assert.True(actual.Version.Value.Equals("0.2.2.0"), $"Expected <0.2.2.0>, got <{actual.Version.Value}>");
        }

        // TEST: fails with wrong version
        // TEST: fails with package id
    }
}