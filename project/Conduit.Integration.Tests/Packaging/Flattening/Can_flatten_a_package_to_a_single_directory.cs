using System.Collections.Generic;
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
                targetDirectory, 
                new NugetPackage("Conduit.Build.Targets", FrameworkVersion.Net45));

            Assert.Equal(3, result.Count);

            targetDirectory.MustContain(
                "Conduit.Adapters.Build.dll", 
                "Conduit.Build.Targets.dll", 
                "Conduit.dll");
        }

        //[Fact]
        //public void and_multiples()
        //{
        //    var packagesDir = new DirectoryInfo("packages");

        //    Nuget.Install(Settings.PublicNuget, "Conduit.Build.Targets" , "0.0.8", packagesDir);
        //    Nuget.Install(Settings.PublicNuget, "EntityFramework"       , "1.0.0", packagesDir);

        //    var targetDirectory = new DirectoryInfo("bin");

        //    var result = Nuget.Flatten(
        //        Settings.PublicNuget,
        //        packagesDir,
        //        "Conduit.Build.Targets",
        //        FrameworkVersion.Net45,
        //        targetDirectory);

        //    Assert.Equal(3, result.Count);

        //    targetDirectory.MustContain(
        //        "Conduit.Adapters.Build.dll",
        //        "Conduit.Build.Targets.dll",
        //        "Conduit.dll");
        //}

        // TEST: it creates target dir is required
        // TEST: it returns nothing if framework version is incompatible
    }
}