using System;
using System.IO;
using System.Threading;
using Conduit.Adapters.Build;
using Conduit.Integration.Tests.Support;
using Xunit;
using NuGet;
using System.Collections.Generic;
using System.Linq;

namespace Conduit.Integration.Tests
{
    public class Examples : RunsInCleanRoom
    {
        /*

        The idea here is to allow selection of a bunch of assemblies from a set of nuget packages so that you can put them all in one directory.

        This means all dependencies can be satisfied.

        IDEs call this "copy local"

        */

        [Fact]
        public void the_basics() 
        {
            string packageID = "EntityFramework";

            var repo = NuGet.PackageRepositoryFactory.Default.CreateRepository ("https://packages.nuget.org/api/v2");

            var packages = repo.FindPackagesById(packageID).ToList();

            Assert.True (packages.Count > 0);
        }

        [Fact]
        public void install_it() 
        {
            var repo = NuGet.PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");

            var targetDir = new DirectoryInfo($"packages-{System.Guid.NewGuid()}");
            targetDir.Create();

            var packageManager = new PackageManager(repo, targetDir.FullName);

            packageManager.InstallPackage("Conduit.Build.Targets", SemanticVersion.Parse ("0.0.8"));

            Assert.True(targetDir.Exists);
        }
    }

    internal static class Nugget {
        internal static FileInfo[] Flatten(DirectoryInfo packageDirectory, FrameworkVersion version, DirectoryInfo targetDirectory) 
        {

            return targetDirectory.GetFiles();
        }
    }

    public class FrameworkVersion {
        public static readonly FrameworkVersion Net45 = new FrameworkVersion("net45") ;
        public string Name { get; private set;}
        public FrameworkVersion(string name)
        {
            Name = name;
        }
    }
}

