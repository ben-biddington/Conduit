using System;
using System.IO;
using System.Linq;
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
                new NugetPackage("Minimatch", new PackageVersion("1.1.0"), FrameworkNames.Net45),
                new NugetPackage("Conduit.Build.Targets", new PackageVersion("0.2.2"), FrameworkNames.Net45)
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
        public void for_example_this_has_a_version()
        {
            var actual = LocalPackages.Find(Settings.PublicNuget, _samples.PackagesDir, "Conduit.Build.Targets");

            Assert.True(actual.Id.Equals("Conduit.Build.Targets"));
            Assert.Equal(FrameworkNames.Net45, actual.FrameworkNames.First());
            Assert.True(actual.Version.Value.Equals("0.2.2.0"), $"Expected <0.2.2.0>, got <{actual.Version.Value}>");
        }

        [Fact]
        public void but_this_one_has_no_framework_version_set_at_all()
        {
            var actual = LocalPackages.Find(Settings.PublicNuget, _samples.PackagesDir, "Minimatch");

            Assert.True(actual.Id.Equals("Minimatch"));
            Assert.Equal(new Version("4.0"), actual.FrameworkNames.First().Version);
            Assert.True(actual.Version.Value.Equals("1.1.0.0"), $"Expected <1.1.0.0>, got <{actual.Version.Value}>");
        }

        // TEST: fails with wrong version
        // TEST: fails with package id
    }
}