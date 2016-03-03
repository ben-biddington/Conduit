using System.Collections.Generic;
using System.IO;
using Conduit.Adapters.Build.Packaging;
using Conduit.Integration.Tests.Support;
using Xunit;
using System.Linq;

namespace Conduit.Integration.Tests.Packaging.Flattening
{
    public class Can_read_a_packages_config_file : RunsInCleanRoom
    {
        [Fact]
        public void for_example() {
            var xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <packages>
                            <package id=""Microsoft.Web.Xdt"" version=""2.1.0"" targetFramework=""net45"" />
                            <package id=""Newtonsoft.Json"" version=""6.0.4"" targetFramework=""net45"" />
                            <package id=""xunit"" version=""2.1.0"" targetFramework=""net45"" />
                            <package id=""xunit.core"" version=""2.1.0"" targetFramework=""net45"" />
                        </packages>";

            var file = FileMachine.Make("packages.config", xml);

            var packages = PackagesConfig.Read(file);

            Assert.True(packages.Any(it => it.Id.Equals("xunit")), string.Join(",", packages.Select(it => it.Id)));
        }

        [Fact]
        public void it_returns_empty_when_file_does_not_exist() {
            var packages = PackagesConfig.Read(new FileInfo("xxx-does-not-exist-xxx"));

            Assert.Equal(0, packages.Count);
        }

        // TEST: Should really be able to supply text rather than file
        // TEST: Check that it returns all the right values
    }

    public class Can_flatten_a_package_to_a_single_directory : RunsInCleanRoom
    {
        [Fact]
        public void for_example_a_single_package()
        {
            var packagesDir = new DirectoryInfo("packages");

            Nuget.Install(Settings.PublicNuget, packagesDir, new NugetPackage("Conduit.Build.Targets", new PackageVersion("0.0.8"), FrameworkVersion.Net45));

            var targetDirectory = new DirectoryInfo("bin");

            var result = Nuget.Flatten(
                Settings.PublicNuget, 
                packagesDir, 
                targetDirectory, 
                new NugetPackage("Conduit.Build.Targets", new PackageVersion("0.0.8"), FrameworkVersion.Net45));

            Assert.Equal(3, result.Count);

            targetDirectory.MustContain(
                "Conduit.Adapters.Build.dll", 
                "Conduit.Build.Targets.dll", 
                "Conduit.dll");
        }

        [Fact]
        public void and_multiples()
        {
            var packagesDir = new DirectoryInfo("packages");

            var packages = new[]
            {
                new NugetPackage("Conduit.Build.Targets", new PackageVersion("0.0.8"), FrameworkVersion.Net45),
                new NugetPackage("EntityFramework"      , new PackageVersion("6.0.0"), FrameworkVersion.Net45),
            };

            Nuget.Install(Settings.PublicNuget, packagesDir, packages);

            var targetDirectory = new DirectoryInfo("bin");

            var result = Nuget.Flatten(
                Settings.PublicNuget,
                packagesDir,
                targetDirectory, 
                packages);

            Assert.Equal(7, result.Count);

            targetDirectory.MustContain(
                "Conduit.Adapters.Build.dll",
                "Conduit.Build.Targets.dll",
                "Conduit.dll",
                "EntityFramework.SqlServer.dll",
                "EntityFramework.SqlServer.xml",
                "EntityFramework.dll",
                "EntityFramework.xml");
        }

        // TEST: it creates target dir if required
        // TEST: it returns nothing if framework version is incompatible
        // TEST: it what when no packages are supplied?
        // TEST: what about packages like xunit.msbuild that does not have lib/net45? Take the first
    }
}   