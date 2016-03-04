using System.IO;
using Conduit.Adapters.Build.Packaging;
using Conduit.Integration.Tests.Support;
using Xunit;

namespace Conduit.Integration.Tests.Packaging.Flattening
{
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