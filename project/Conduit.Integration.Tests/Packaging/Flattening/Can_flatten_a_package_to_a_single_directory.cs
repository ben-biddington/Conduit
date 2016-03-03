using System;
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

            Nuget.Install(Settings.PublicNuget, "Conduit.Build.Targets", "0.0.8", packagesDir);

            var targetDirectory = new DirectoryInfo("bin");

            var result = Nuget.Flatten(
                Settings.PublicNuget, 
                packagesDir, 
                "Conduit.Build.Targets", 
                FrameworkVersion.Net45,
                targetDirectory);

            Assert.Equal(3, result.Count);

            targetDirectory.MustContain("Conduit.Build.Targets.dll");
        }

        // TEST: it creates target dir is required
        // TEST: it returns nothing if framework version is incompatible
    }

    internal static class DirectoryInfoAssertions
    {
        internal static void MustContain(this DirectoryInfo self, params string[] expectedFiles)
        {
            var files = self.GetFiles();

            Assert.True(files.Length == expectedFiles.Length,
                $"Expected the dir <{self.FullName}> to contain <{expectedFiles.Length}> files, but it contains <{files.Length}>.");
        }
    }

    static class Settings
    {
        public static Uri PublicNuget => new Uri("https://packages.nuget.org/api/v2");
    }
}